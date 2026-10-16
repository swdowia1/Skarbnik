using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Skarbnik.DB;
using Skarbnik.VM;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Skarbnik.Pages
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> OnGetTestExcelAsync()
        {
            using var package = new ExcelPackage();

            // ==========================================
            // POBRANIE DANYCH
            // ==========================================

            var uczniowie = await _context.Uczniowie
                .OrderBy(u => u.Id)
                .ToListAsync();

            var wydatki = await _context.Wydatki
                .OrderBy(w => w.Nazwa)
                .ToListAsync();

            var przypisania = await _context.UczenWydatki
                .ToListAsync();

            var przypisaniaSlownik = przypisania.ToDictionary(
                x => (x.UczenId, x.WydatekId));

            // ==========================================
            // ARKUSZ 1: PODSUMOWANIE
            // ==========================================

            var podsumowanie = package.Workbook.Worksheets.Add("Podsumowanie");

            int kolumnaSuma = wydatki.Count + 2;
            int kolumnaZaleglosc = wydatki.Count + 3;
            int ostatniaKolumna = kolumnaZaleglosc;

            int wierszRazem = uczniowie.Count + 2;
            int wierszNiezaplacono = uczniowie.Count + 3;

            // Nagłówki
            podsumowanie.Cells[1, 1].Value = "Uczeń";

            for (int j = 0; j < wydatki.Count; j++)
            {
                podsumowanie.Cells[1, j + 2].Value = wydatki[j].Nazwa;
            }

            podsumowanie.Cells[1, kolumnaSuma].Value = "Suma przypisanych";
            podsumowanie.Cells[1, kolumnaZaleglosc].Value = "Nie zapłacono";

            // Sumy per wydatek
            var sumaPerWydatek = new decimal[wydatki.Count];
            var zalegloscPerWydatek = new decimal[wydatki.Count];

            decimal sumaWszystkichWydatkow = 0;
            decimal sumaWszystkichZaleglosci = 0;

            // Wiersze uczniów
            for (int i = 0; i < uczniowie.Count; i++)
            {
                var uczen = uczniowie[i];
                int wiersz = i + 2;

                podsumowanie.Cells[wiersz, 1].Value =
                    $"{uczen.Nazwisko}";

                decimal sumaUcznia = 0;
                decimal zalegloscUcznia = 0;

                for (int j = 0; j < wydatki.Count; j++)
                {
                    var wydatek = wydatki[j];
                    int kolumna = j + 2;

                    var komorka = podsumowanie.Cells[wiersz, kolumna];

                    if (!przypisaniaSlownik.TryGetValue(
                            (uczen.Id, wydatek.Id), out var przypisanie))
                    {
                        komorka.Value = "Nie dotyczy";
                        continue;
                    }

                    // Każde przypisanie wliczamy do sumy
                    sumaUcznia += wydatek.Kwota;
                    sumaPerWydatek[j] += wydatek.Kwota;
                    sumaWszystkichWydatkow += wydatek.Kwota;

                    if (przypisanie.CzyZaplacono)
                    {
                        komorka.Value = wydatek.Kwota;

                    }
                    else
                    {
                        komorka.Value = wydatek.Kwota;

                        komorka.Style.Font.Color.SetColor(System.Drawing.Color.Red);

                        zalegloscUcznia += wydatek.Kwota;
                        zalegloscPerWydatek[j] += wydatek.Kwota;
                        sumaWszystkichZaleglosci += wydatek.Kwota;
                    }

                    komorka.Style.Numberformat.Format = "#,##0.00 \"zł\"";
                    komorka.Style.HorizontalAlignment =
                        ExcelHorizontalAlignment.Right;
                }

                // Sumy per uczeń
                podsumowanie.Cells[wiersz, kolumnaSuma].Value = sumaUcznia;
                podsumowanie.Cells[wiersz, kolumnaZaleglosc].Value = zalegloscUcznia;
            }

            // ==========================================
            // WIERSZ: RAZEM
            // ==========================================

            podsumowanie.Cells[wierszRazem, 1].Value = "RAZEM";

            for (int j = 0; j < wydatki.Count; j++)
            {
                podsumowanie.Cells[wierszRazem, j + 2].Value =
                    sumaPerWydatek[j];
            }

            podsumowanie.Cells[wierszRazem, kolumnaSuma].Value =
                sumaWszystkichWydatkow;

            podsumowanie.Cells[wierszRazem, kolumnaZaleglosc].Value =
                sumaWszystkichZaleglosci;

            // ==========================================
            // WIERSZ: NIE ZAPŁACONO
            // ==========================================

            podsumowanie.Cells[wierszNiezaplacono, 1].Value =
                "NIE ZAPŁACONO";

            for (int j = 0; j < wydatki.Count; j++)
            {
                podsumowanie.Cells[wierszNiezaplacono, j + 2].Value =
                    zalegloscPerWydatek[j];
            }

            podsumowanie.Cells[wierszNiezaplacono, kolumnaSuma].Value =
                sumaWszystkichZaleglosci;

            podsumowanie.Cells[wierszNiezaplacono, kolumnaZaleglosc].Value =
                sumaWszystkichZaleglosci;

            // ==========================================
            // FORMATOWANIE ARKUSZA PODSUMOWANIE
            // ==========================================

            var zakresTabeli = podsumowanie.Cells[
                1, 1, wierszNiezaplacono, ostatniaKolumna];

            // Obramowanie tabeli
            zakresTabeli.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            zakresTabeli.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            zakresTabeli.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            zakresTabeli.Style.Border.Right.Style = ExcelBorderStyle.Thin;

            // Nagłówek
            using (var naglowek = podsumowanie.Cells[
                1, 1, 1, ostatniaKolumna])
            {
                naglowek.Style.Font.Bold = true;
                naglowek.Style.WrapText = true;
                naglowek.Style.HorizontalAlignment =
                    ExcelHorizontalAlignment.Center;
                naglowek.Style.VerticalAlignment =
                    ExcelVerticalAlignment.Center;
            }

            // Wiersz RAZEM
            using (var zakres = podsumowanie.Cells[
                wierszRazem, 1, wierszRazem, ostatniaKolumna])
            {
                zakres.Style.Font.Bold = true;
                zakres.Style.Border.Top.Style = ExcelBorderStyle.Medium;
            }

            // Wiersz NIE ZAPŁACONO
            using (var zakres = podsumowanie.Cells[
                wierszNiezaplacono, 1,
                wierszNiezaplacono, ostatniaKolumna])
            {
                zakres.Style.Font.Bold = true;
                
                zakres.Style.Font.Color.SetColor(System.Drawing.Color.Red);
                zakres.Style.Border.Top.Style = ExcelBorderStyle.Medium;
            }

            // Format kwot w tabeli i podsumowaniach
            podsumowanie.Cells[
                2, 2, wierszNiezaplacono, ostatniaKolumna]
                .Style.Numberformat.Format = "#,##0.00 \"zł\"";

            zakresTabeli.Style.VerticalAlignment =
                ExcelVerticalAlignment.Center;

            podsumowanie.Column(1).Width = 25;

            for (int j = 0; j < wydatki.Count; j++)
            {
                podsumowanie.Column(j + 2).Width = 20;
            }

            podsumowanie.Column(kolumnaSuma).Width = 22;
            podsumowanie.Column(kolumnaZaleglosc).Width = 20;

            podsumowanie.Row(1).Height = 35;
            podsumowanie.View.FreezePanes(2, 2);

            // ==========================================
            // ARKUSZ 2: LISTA WYDATKÓW
            // ==========================================

            var lista = package.Workbook.Worksheets.Add("Lista wydatków");

            lista.Cells["A1"].Value = "Uczeń";
            lista.Cells["B1"].Value = "Nazwa wydatku";
            lista.Cells["C1"].Value = "Kwota";
            lista.Cells["D1"].Value = "Status";
            lista.Cells["E1"].Value = "Data zapłaty";

            int numerWiersza = 2;

            foreach (var uczen in uczniowie)
            {
                foreach (var wydatek in wydatki)
                {
                    przypisaniaSlownik.TryGetValue(
                        (uczen.Id, wydatek.Id), out var przypisanie);

                    string status;

                    if (przypisanie == null)
                    {
                        status = "Nie dotyczy";
                    }
                    else if (przypisanie.CzyZaplacono)
                    {
                        status = "Zapłacono";
                    }
                    else
                    {
                        status = "Niezapłacono";
                    }

                    lista.Cells[numerWiersza, 1].Value =
                        $"{uczen.Nazwisko}";

                    lista.Cells[numerWiersza, 2].Value = wydatek.Nazwa;
                    lista.Cells[numerWiersza, 3].Value = wydatek.Kwota;
                    lista.Cells[numerWiersza, 4].Value = status;

                    // Data tylko dla opłaconego wydatku
                    if (przypisanie?.CzyZaplacono == true &&
                        przypisanie.DataZaplaty.HasValue)
                    {
                        lista.Cells[numerWiersza, 5].Value =
                            przypisanie.DataZaplaty.Value;
                    }

                    // Kolor czcionki statusu, bez kolorowania tła
                    var komorkaStatusu = lista.Cells[numerWiersza, 4];

                    if (status == "Niezapłacono")
                    {
                        komorkaStatusu.Style.Font.Color.SetColor(System.Drawing.Color.Red);
                    }
                   

                    numerWiersza++;
                }
            }

            // ==========================================
            // FORMATOWANIE ARKUSZA LISTA WYDATKÓW
            // ==========================================

            var zakresListy = lista.Cells[
                1, 1, numerWiersza - 1, 5];

            using (var naglowek = lista.Cells["A1:E1"])
            {
                naglowek.Style.Font.Bold = true;
                naglowek.Style.WrapText = true;
                naglowek.Style.HorizontalAlignment =
                    ExcelHorizontalAlignment.Center;
                naglowek.Style.VerticalAlignment =
                    ExcelVerticalAlignment.Center;
            }

            zakresListy.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            zakresListy.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            zakresListy.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            zakresListy.Style.Border.Right.Style = ExcelBorderStyle.Thin;

            lista.Cells[1, 1, numerWiersza - 1, 5].AutoFilter = true;

            lista.Column(1).Width = 25;
            lista.Column(2).Width = 35;
            lista.Column(3).Width = 15;
            lista.Column(4).Width = 20;
            lista.Column(5).Width = 18;

            lista.Column(3).Style.Numberformat.Format = "#,##0.00 \"zł\"";
            lista.Column(5).Style.Numberformat.Format = "dd.MM.yyyy";

            lista.Row(1).Height = 25;
            lista.View.FreezePanes(2, 1);

            // ==========================================
            // ZWRÓCENIE PLIKU
            // ==========================================

            var plik = package.GetAsByteArray();

            return File(
                plik,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"Skarbnik_Wydatki_{DateTime.Now:yyyyMMdd}.xlsx");
        }
       
        public PodsumowanieVM Podsumowanie { get; set; } = new();

        public async Task OnGetAsync()
        {
            var uczniowie = await _context.Uczniowie
                .AsNoTracking()
                .OrderBy(u => u.Nazwisko)
                .Select(u => new
                {
                    u.Id,
                    u.Nazwisko
            
                })
                .ToListAsync();

            var wydatki = await _context.Wydatki
                .AsNoTracking()
                .OrderByDescending(w => w.CreatedAt)
                .Select(w => new
                {
                    w.Id,
                    w.Nazwa,
                    w.Kwota
                })
                .ToListAsync();

            var przypisania = await _context.UczenWydatki
                .AsNoTracking()
                .Select(uw => new
                {
                    id=uw.Id,
                    uw.UczenId,
                    uw.WydatekId,
                    uw.CzyZaplacono,
                    Kwota = uw.Wydatek.Kwota
                })                      
                .ToListAsync();

            var mapa = przypisania.ToDictionary(
                x => (x.UczenId, x.WydatekId));

            Podsumowanie = new PodsumowanieVM();

            foreach (var wydatek in wydatki)
            {
                var przypisane = przypisania
                    .Where(p => p.WydatekId == wydatek.Id)
                    .ToList();

                Podsumowanie.Wydatki.Add(new WydatekKolumnaVM
                {
                    Id = wydatek.Id,
                    Nazwa = wydatek.Nazwa,
                    Kwota = wydatek.Kwota,
                    Suma = przypisane.Sum(p => p.Kwota),

                    SumaNiezaplacona = przypisane
                        .Where(p => !p.CzyZaplacono)
                        .Sum(p => p.Kwota)
                });
            }

            foreach (var uczen in uczniowie)
            {
                var wiersz = new UczenPodsumowanieVM
                {
                    Id = uczen.Id,
                    Nazwisko = $"{uczen.Nazwisko}"
                };

                foreach (var wydatek in wydatki)
                {
                    if (mapa.TryGetValue(
                        (uczen.Id, wydatek.Id), out var przypisanie))
                    {
                        wiersz.Wydatki.Add(new KomorkaWydatkuVM
                        {
                            Id = przypisanie.id,
                            Kwota = przypisanie.Kwota,
                            CzyZaplacono = przypisanie.CzyZaplacono,
                            Nazwa = wydatek.Nazwa

                        });

                        if (!przypisanie.CzyZaplacono)
                        {
                            wiersz.SumaNiezaplacona +=
                                przypisanie.Kwota;
                        }
                    }
                    else
                    {
                        wiersz.Wydatki.Add(new KomorkaWydatkuVM
                        {
                            Kwota = null,
                            UczenId = uczen.Id,
                            WydatekId = wydatek.Id,
                            Nazwa = wydatek.Nazwa
                        });
                    }
                }

                Podsumowanie.Uczniowie.Add(wiersz);
            }
        }
    }
}
