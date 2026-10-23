using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Skarbnik.DB;
using Skarbnik.Enums;
using Skarbnik.Services;
using Xunit;

namespace Skarbnik.Tests;

public sealed class UczenServiceTests : IAsyncLifetime
{
    private SqliteConnection _connection = null!;
    private AppDbContext _context = null!;
    private UczenService _service = null!;

    public async Task InitializeAsync()
    {
        _connection = new SqliteConnection("Data Source=:memory:");
        await _connection.OpenAsync();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(_connection)
            .Options;

        _context = new AppDbContext(options);
        await _context.Database.EnsureCreatedAsync();
        _service = new UczenService(_context);
    }

    public async Task DisposeAsync()
    {
        await _context.DisposeAsync();
        await _connection.DisposeAsync();
    }

    [Fact]
    public async Task AktualizujAsync_ZapisujeZmianyIstniejacegoUcznia()
    {
        var uczen = await DodajUczniaAsync();

        var wynik = await _service.AktualizujAsync(new Uczen
        {
            Id = uczen.Id,
            Nazwisko = "Nowak",
            Skrot = "N"
        });

        Assert.True(wynik);
        Assert.Equal("Nowak", uczen.Nazwisko);
        Assert.Equal("N", uczen.Skrot);
    }

    [Fact]
    public async Task AktualizujAsync_ZwracaFalseDlaNieistniejacegoUcznia()
    {
        var wynik = await _service.AktualizujAsync(new Uczen
        {
            Id = 999,
            Nazwisko = "Nowak",
            Skrot = "N"
        });

        Assert.False(wynik);
        Assert.Empty(await _context.Uczniowie.ToListAsync());
    }

    [Fact]
    public async Task PobierzWszystkichAsync_SortujeUczniowPoNazwisku()
    {
        await DodajUczniaAsync();
        _context.Uczniowie.Add(new Uczen { Nazwisko = "Adamski", Skrot = "A" });
        await _context.SaveChangesAsync();

        var uczniowie = await _service.PobierzWszystkichAsync();

        Assert.Equal(new[] { "Adamski", "Kowalski" }, uczniowie.Select(uczen => uczen.Nazwisko));
    }

    [Fact]
    public async Task PrzypiszWydatekAsync_DodajeNieoplaconePrzypisanie()
    {
        var (uczen, wydatek) = await DodajUczniaIWydatekAsync();

        var wynik = await _service.PrzypiszWydatekAsync(uczen.Id, wydatek.Id);

        Assert.True(wynik);
        var przypisanie = await _context.UczenWydatki.SingleAsync();
        Assert.False(przypisanie.CzyZaplacono);
        Assert.Null(przypisanie.DataZaplaty);
    }

    [Fact]
    public async Task PrzypiszWydatekAsync_DlaOplaconegoUstawiaDateZaplaty()
    {
        var (uczen, wydatek) = await DodajUczniaIWydatekAsync();

        var wynik = await _service.PrzypiszWydatekAsync(
            uczen.Id, wydatek.Id, czyZaplacono: true);

        Assert.True(wynik);
        var przypisanie = await _context.UczenWydatki.SingleAsync();
        Assert.True(przypisanie.CzyZaplacono);
        Assert.NotNull(przypisanie.DataZaplaty);
        Assert.Equal(DateTimeKind.Utc, przypisanie.DataZaplaty!.Value.Kind);
    }

    [Fact]
    public async Task PrzypiszWszystkichUczniowAsync_PrzypisujeTylkoBrakujacych()
    {
        var pierwszyUczen = await DodajUczniaAsync();
        var drugiUczen = await DodajUczniaAsync();
        var wydatek = await DodajWydatekAsync();
        Assert.True(await _service.PrzypiszWydatekAsync(pierwszyUczen.Id, wydatek.Id, czyZaplacono: true));

        var liczbaDodanych = await _service.PrzypiszWszystkichUczniowAsync(wydatek.Id);
        var przypisania = await _context.UczenWydatki.OrderBy(x => x.UczenId).ToListAsync();

        Assert.Equal(1, liczbaDodanych);
        Assert.Equal(new[] { pierwszyUczen.Id, drugiUczen.Id }, przypisania.Select(x => x.UczenId));
        Assert.True(przypisania[0].CzyZaplacono);
        Assert.False(przypisania[1].CzyZaplacono);
    }

    [Fact]
    public async Task PrzypiszWszystkichUczniowAsync_ZwracaNullDlaNieistniejacegoWydatku()
    {
        var wynik = await _service.PrzypiszWszystkichUczniowAsync(999);

        Assert.Null(wynik);
    }

    [Fact]
    public async Task UsunWydatekAsync_UsuwaWydatekOrazPrzypisania()
    {
        var (uczen, wydatek) = await DodajUczniaIWydatekAsync();
        Assert.True(await _service.PrzypiszWydatekAsync(uczen.Id, wydatek.Id));

        var wynik = await _service.UsunWydatekAsync(wydatek.Id);

        Assert.True(wynik);
        Assert.Empty(await _context.Wydatki.ToListAsync());
        Assert.Empty(await _context.UczenWydatki.ToListAsync());
    }

    [Fact]
    public async Task PrzypiszWydatekAsync_OdrzucaNieistniejacegoUcznia()
    {
        var wydatek = await DodajWydatekAsync();

        var wynik = await _service.PrzypiszWydatekAsync(999, wydatek.Id);

        Assert.False(wynik);
        Assert.Empty(await _context.UczenWydatki.ToListAsync());
    }

    [Fact]
    public async Task PrzypiszWydatekAsync_OdrzucaNieistniejacyWydatek()
    {
        var uczen = await DodajUczniaAsync();

        var wynik = await _service.PrzypiszWydatekAsync(uczen.Id, 999);

        Assert.False(wynik);
        Assert.Empty(await _context.UczenWydatki.ToListAsync());
    }

    [Fact]
    public async Task PrzypiszWydatekAsync_OdrzucaPowtornePrzypisanie()
    {
        var (uczen, wydatek) = await DodajUczniaIWydatekAsync();
        Assert.True(await _service.PrzypiszWydatekAsync(uczen.Id, wydatek.Id));

        var wynik = await _service.PrzypiszWydatekAsync(uczen.Id, wydatek.Id);

        Assert.False(wynik);
        Assert.Single(await _context.UczenWydatki.ToListAsync());
    }

    [Fact]
    public async Task UstawCzyZaplaconoAsync_UstawiaICofaStatusWrazZData()
    {
        var (uczen, wydatek) = await DodajUczniaIWydatekAsync();
        Assert.True(await _service.PrzypiszWydatekAsync(uczen.Id, wydatek.Id));
        var przypisanie = await _context.UczenWydatki.SingleAsync();

        var oplacono = await _service.UstawCzyZaplaconoAsync(
            przypisanie.Id, czyZaplacono: true);

        Assert.True(oplacono);
        Assert.True(przypisanie.CzyZaplacono);
        Assert.NotNull(przypisanie.DataZaplaty);

        var cofnieto = await _service.UstawCzyZaplaconoAsync(
            przypisanie.Id, czyZaplacono: false);

        Assert.True(cofnieto);
        Assert.False(przypisanie.CzyZaplacono);
        Assert.Null(przypisanie.DataZaplaty);
    }

    [Fact]
    public async Task PobierzStatusyUczniowWydatkuAsync_ZwracaBrakDlaNieprzypisanegoUcznia()
    {
        var uczen = await DodajUczniaAsync();
        var wydatek = await DodajWydatekAsync();

        var statusy = await _service.PobierzStatusyUczniowWydatkuAsync(wydatek.Id);

        var status = Assert.Single(statusy);
        Assert.Equal(uczen.Id, status.UczenId);
        Assert.Equal(StanPlatnosc.Brak, status.StanPlatnosc);
        Assert.Null(status.DataZaplaty);
    }

    [Fact]
    public async Task PobierzNiezaplaconeAsync_SortujeDeterministyczniePoIdPrzyRemisach()
    {
        var pierwszyUczen = await DodajUczniaAsync();
        var drugiUczen = await DodajUczniaAsync();
        var pierwszyWydatek = await DodajWydatekAsync();
        var drugiWydatek = await DodajWydatekAsync();

        _context.UczenWydatki.AddRange(
            new UczenWydatek { UczenId = drugiUczen.Id, WydatekId = drugiWydatek.Id },
            new UczenWydatek { UczenId = drugiUczen.Id, WydatekId = pierwszyWydatek.Id },
            new UczenWydatek { UczenId = pierwszyUczen.Id, WydatekId = drugiWydatek.Id },
            new UczenWydatek { UczenId = pierwszyUczen.Id, WydatekId = pierwszyWydatek.Id });
        await _context.SaveChangesAsync();

        var pozycje = await _service.PobierzNiezaplaconeAsync();

        Assert.Equal(
            new[]
            {
                (pierwszyUczen.Id, pierwszyWydatek.Id),
                (pierwszyUczen.Id, drugiWydatek.Id),
                (drugiUczen.Id, pierwszyWydatek.Id),
                (drugiUczen.Id, drugiWydatek.Id)
            },
            pozycje.Select(p => (p.UczenId, p.WydatekId)));
    }

    private async Task<(Uczen Uczen, Wydatek Wydatek)> DodajUczniaIWydatekAsync()
    {
        var uczen = await DodajUczniaAsync();
        var wydatek = await DodajWydatekAsync();
        return (uczen, wydatek);
    }

    private async Task<Uczen> DodajUczniaAsync()
    {
        var uczen = new Uczen { Nazwisko = "Kowalski", Skrot = "K" };
        _context.Uczniowie.Add(uczen);
        await _context.SaveChangesAsync();
        return uczen;
    }

    private async Task<Wydatek> DodajWydatekAsync()
    {
        var wydatek = new Wydatek { Nazwa = "Składka", Kwota = 25m };
        _context.Wydatki.Add(wydatek);
        await _context.SaveChangesAsync();
        return wydatek;
    }
}
