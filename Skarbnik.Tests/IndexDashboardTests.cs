using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Skarbnik.DB;
using Skarbnik.Pages;
using Skarbnik.Services;
using Xunit;

namespace Skarbnik.Tests;

public sealed class IndexDashboardTests : IAsyncLifetime
{
    private SqliteConnection _connection = null!;
    private AppDbContext _context = null!;

    public async Task InitializeAsync()
    {
        _connection = new SqliteConnection("Data Source=:memory:");
        await _connection.OpenAsync();
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(_connection)
            .Options;
        _context = new AppDbContext(options);
        await _context.Database.EnsureCreatedAsync();
    }

    public async Task DisposeAsync()
    {
        await _context.DisposeAsync();
        await _connection.DisposeAsync();
    }

    [Fact]
    public async Task OnGetAsync_LiczyStanKasyIZwracaOstatnieWydatkiIWplaty()
    {
        var kowalski = new Uczen { Nazwisko = "Kowalski", Skrot = "K" };
        var nowak = new Uczen { Nazwisko = "Nowak", Skrot = "N" };
        var starszy = new Wydatek
        {
            Nazwa = "Starszy wydatek",
            Kwota = 20m,
            CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        };
        var nowszy = new Wydatek
        {
            Nazwa = "Nowszy wydatek",
            Kwota = 30m,
            CreatedAt = new DateTime(2026, 2, 1, 0, 0, 0, DateTimeKind.Utc)
        };
        _context.AddRange(kowalski, nowak, starszy, nowszy);
        await _context.SaveChangesAsync();
        _context.UczenWydatki.AddRange(
            new UczenWydatek
            {
                UczenId = kowalski.Id,
                WydatekId = starszy.Id,
                CzyZaplacono = true,
                DataZaplaty = new DateTime(2026, 3, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new UczenWydatek
            {
                UczenId = kowalski.Id,
                WydatekId = nowszy.Id,
                CzyZaplacono = false
            },
            new UczenWydatek
            {
                UczenId = nowak.Id,
                WydatekId = nowszy.Id,
                CzyZaplacono = false
            });
        await _context.SaveChangesAsync();

        var model = new DashboardModel(new UczenService(_context));
        await model.OnGetAsync();

        Assert.Equal(80m, model.SumaNaleznosci);
        Assert.Equal(20m, model.SumaWplacona);
        Assert.Equal(2, model.LiczbaUczniowZZaleglosciami);
        Assert.Equal(new[] { "Nowszy wydatek", "Starszy wydatek" },
            model.OstatnieWydatki.Select(w => w.Nazwa));
        var wplata = Assert.Single(model.OstatnieWplaty);
        Assert.Equal("Kowalski", wplata.Uczen);
        Assert.Equal("Starszy wydatek", wplata.Nazwa);
        Assert.Equal(20m, wplata.Kwota);
    }
}
