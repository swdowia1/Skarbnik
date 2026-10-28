using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Skarbnik.DB;
using Skarbnik.Pages;
using Skarbnik.Services;
using System.Text;
using Xunit;

namespace Skarbnik.Tests;

public sealed class CsvExportTests : IAsyncLifetime
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
    public async Task EksportCsv_ZawieraNaglowkiKwotyStatusyOrazBrakPrzypisania()
    {
        var kowalski = new Uczen { Nazwisko = "Kowalski", Skrot = "K" };
        var nowak = new Uczen { Nazwisko = "Nowak", Skrot = "N" };
        var skladka = new Wydatek
        {
            Nazwa = "Składka",
            Kwota = 25.50m,
            CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        };
        var wycieczka = new Wydatek
        {
            Nazwa = "Wycieczka",
            Kwota = 120m,
            CreatedAt = new DateTime(2026, 1, 2, 0, 0, 0, DateTimeKind.Utc)
        };

        _context.AddRange(kowalski, nowak, skladka, wycieczka);
        await _context.SaveChangesAsync();
        _context.UczenWydatki.AddRange(
            new UczenWydatek
            {
                UczenId = kowalski.Id,
                WydatekId = skladka.Id,
                CzyZaplacono = true,
                DataZaplaty = new DateTime(2026, 5, 15, 12, 0, 0, DateTimeKind.Utc)
            },
            new UczenWydatek
            {
                UczenId = nowak.Id,
                WydatekId = wycieczka.Id,
                CzyZaplacono = false
            });
        await _context.SaveChangesAsync();

        var (wynik, csv) = await PobierzCsvAsync();

        Assert.Equal("text/csv; charset=utf-8", wynik.ContentType);
        Assert.StartsWith("Skarbnik_Wydatki_", wynik.FileDownloadName);
        Assert.EndsWith(".csv", wynik.FileDownloadName);
        Assert.Equal(new byte[] { 0xEF, 0xBB, 0xBF }, wynik.FileContents.Take(3));
        Assert.Contains(
            "\"Uczeń\";\"Wydatek\";\"Kwota (zł)\";\"Status płatności\";\"Data zapłaty\"\r\n",
            csv);
        Assert.Contains("\"Kowalski\";\"Wycieczka\";\"\";\"Brak\";\"\"\r\n", csv);
        Assert.Contains("\"Kowalski\";\"Składka\";\"25,50\";\"Zapłacono\";\"15.05.2026\"\r\n", csv);
        Assert.Contains("\"Nowak\";\"Wycieczka\";\"120,00\";\"Niezapłacono\";\"\"\r\n", csv);
        Assert.Contains("\"Nowak\";\"Składka\";\"\";\"Brak\";\"\"\r\n", csv);
    }

    [Fact]
    public async Task EksportCsv_CytujeSeparatorCudzyslowNowaLinieIPolskieZnaki()
    {
        var uczen = new Uczen
        {
            Nazwisko = "Żaneta; \"Łódź\"\nNowak",
            Skrot = "Z"
        };
        var wydatek = new Wydatek
        {
            Nazwa = "Składka; \"na zamek\"\r\npaździernik",
            Kwota = 12.34m
        };
        _context.AddRange(uczen, wydatek);
        await _context.SaveChangesAsync();
        _context.UczenWydatki.Add(new UczenWydatek
        {
            UczenId = uczen.Id,
            WydatekId = wydatek.Id
        });
        await _context.SaveChangesAsync();

        var (_, csv) = await PobierzCsvAsync();

        Assert.Contains(
            "\"Żaneta; \"\"Łódź\"\"\nNowak\";\"Składka; \"\"na zamek\"\"\r\npaździernik\";\"12,34\";\"Niezapłacono\";\"\"\r\n",
            csv);
    }

    [Fact]
    public async Task EksportCsv_ZabezpieczaTekstRozpoczynajacySieOdFormulyExcela()
    {
        var uczen = new Uczen { Nazwisko = "=2+2", Skrot = "F" };
        var wydatek = new Wydatek { Nazwa = "Wydatek", Kwota = 10m };
        _context.AddRange(uczen, wydatek);
        await _context.SaveChangesAsync();
        _context.UczenWydatki.Add(new UczenWydatek
        {
            UczenId = uczen.Id,
            WydatekId = wydatek.Id
        });
        await _context.SaveChangesAsync();

        var (_, csv) = await PobierzCsvAsync();

        Assert.Contains("\"'=2+2\";\"Wydatek\";\"10,00\";\"Niezapłacono\";\"\"\r\n", csv);
    }

    [Theory]
    [InlineData("+1+1", "'+1+1")]
    [InlineData("-1+1", "'-1+1")]
    [InlineData("@SUM(1,1)", "'@SUM(1,1)")]
    [InlineData(" \t=2+2", "' \t=2+2")]
    public async Task EksportCsv_ZabezpieczaRoznePoczatkiFormulExcela(
        string wartosc,
        string oczekiwanaWartosc)
    {
        var uczen = new Uczen { Nazwisko = wartosc, Skrot = "F" };
        var wydatek = new Wydatek { Nazwa = "Wydatek", Kwota = 10m };
        _context.AddRange(uczen, wydatek);
        await _context.SaveChangesAsync();
        _context.UczenWydatki.Add(new UczenWydatek
        {
            UczenId = uczen.Id,
            WydatekId = wydatek.Id
        });
        await _context.SaveChangesAsync();

        var (_, csv) = await PobierzCsvAsync();

        Assert.Contains(
            $"\"{oczekiwanaWartosc}\";\"Wydatek\";\"10,00\";\"Niezapłacono\";\"\"\r\n",
            csv);
    }

    [Fact]
    public async Task EksportCsv_DlaPustegoRaportuZwracaSameNaglowki()
    {
        var (wynik, csv) = await PobierzCsvAsync();

        Assert.Equal(new byte[] { 0xEF, 0xBB, 0xBF }, wynik.FileContents.Take(3));
        Assert.Equal(
            "\"Uczeń\";\"Wydatek\";\"Kwota (zł)\";\"Status płatności\";\"Data zapłaty\"\r\n",
            csv);
    }

    private async Task<(FileContentResult Wynik, string Csv)> PobierzCsvAsync()
    {
        var model = new IndexModel(new UczenService(_context));
        var wynik = Assert.IsType<FileContentResult>(await model.OnGetCsvAsync());
        var csv = Encoding.UTF8.GetString(
            wynik.FileContents,
            Encoding.UTF8.GetPreamble().Length,
            wynik.FileContents.Length - Encoding.UTF8.GetPreamble().Length);

        return (wynik, csv);
    }
}
