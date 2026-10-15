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

            // Pobranie danych
            var uczniowie = await _context.Uczniowie
                .OrderBy(u => u.Id)
                .ToListAsync();

            var wydatki = await _context.Wydatki
                .OrderBy(w => w.Nazwa)
                .ToListAsync();

            var przypisania = await _context.UczenWydatki
                .ToListAsync();

            // Szybkie wyszukiwanie przypisania po uczniu i wydatku
            var przypisaniaSlownik = przypisania.ToDictionary(
                x => (x.UczenId, x.WydatekId));

            // ==========================================
            // ARKUSZ 1: PODSUMOWANIE
            // ==========================================

            var podsumowanie = package.Workbook.Worksheets.Add("Podsumowanie");

            podsumowanie.Cells[1, 1].Value = "Uczeń";

            for (int j = 0; j < wydatki.Count; j++)
            {
                podsumowanie.Cells[1, j + 2].Value = wydatki[j].Nazwa;
            }

            for (int i = 0; i < uczniowie.Count; i++)
            {
                var uczen = uczniowie[i];
                int wiersz = i + 2;

                podsumowanie.Cells[wiersz, 1].Value =
                    $"{uczen.Nazwisko}";

                for (int j = 0; j < wydatki.Count; j++)
                {
                    var wydatek = wydatki[j];
                    int kolumna = j + 2;

                    string status;

                    if (!przypisaniaSlownik.TryGetValue(
                            (uczen.Id, wydatek.Id), out var przypisanie))
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

                    var komorka = podsumowanie.Cells[wiersz, kolumna];
                    komorka.Value = status;

                    // Kolory statusów
                    if (status == "Zapłacono")
                    {
                        komorka.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        komorka.Style.Fill.BackgroundColor.SetColor(
                            System.Drawing.Color.LightGreen);
                    }
                    else if (status == "Niezapłacono")
                    {
                        komorka.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        komorka.Style.Fill.BackgroundColor.SetColor(
                            System.Drawing.Color.LightPink);
                    }
                    else
                    {
                        komorka.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        komorka.Style.Fill.BackgroundColor.SetColor(
                            System.Drawing.Color.Moccasin);
                    }
                }
            }

            // Formatowanie nagłówka pierwszego arkusza
            using (var naglowek = podsumowanie.Cells[
                1, 1, 1, wydatki.Count + 1])
            {
                naglowek.Style.Font.Bold = true;
                naglowek.Style.Fill.PatternType = ExcelFillStyle.Solid;
                naglowek.Style.Fill.BackgroundColor.SetColor(
                    System.Drawing.Color.LightBlue);
                naglowek.Style.WrapText = true;
            }

            podsumowanie.Cells[
                1, 1, uczniowie.Count + 1, wydatki.Count + 1]
                .Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            podsumowanie.Cells[
                1, 1, uczniowie.Count + 1, wydatki.Count + 1]
                .Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            podsumowanie.Column(1).Width = 25;

            for (int j = 0; j < wydatki.Count; j++)
            {
                podsumowanie.Column(j + 2).Width = 20;
            }

            podsumowanie.View.FreezePanes(2, 2);

            // ==========================================
            // ARKUSZ 2: PŁASKA LISTA WYDATKÓW
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

                        lista.Cells[numerWiersza, 5]
                            .Style.Numberformat.Format = "dd.MM.yyyy";
                    }

                    // Kolor statusu
                    var komorkaStatusu = lista.Cells[numerWiersza, 4];
                    komorkaStatusu.Style.Fill.PatternType =
                        ExcelFillStyle.Solid;

                    if (status == "Zapłacono")
                    {
                        komorkaStatusu.Style.Fill.BackgroundColor.SetColor(
                            System.Drawing.Color.LightGreen);
                    }
                    else if (status == "Niezapłacono")
                    {
                        komorkaStatusu.Style.Fill.BackgroundColor.SetColor(
                            System.Drawing.Color.LightPink);
                    }
                    else
                    {
                        komorkaStatusu.Style.Fill.BackgroundColor.SetColor(
                            System.Drawing.Color.Moccasin);
                    }

                    if (przypisanie?.CzyZaplacono == true &&
                        przypisanie.DataZaplaty.HasValue)
                    {
                        lista.Cells[numerWiersza, 5].Style.Numberformat.Format =
                            "dd.MM.yyyy";
                    }

                    numerWiersza++;
                }
            }

            // Formatowanie nagłówka drugiego arkusza
            using (var naglowek = lista.Cells["A1:E1"])
            {
                naglowek.Style.Font.Bold = true;
                naglowek.Style.Fill.PatternType = ExcelFillStyle.Solid;
                naglowek.Style.Fill.BackgroundColor.SetColor(
                    System.Drawing.Color.LightBlue);
            }

            lista.Cells[1, 1, numerWiersza - 1, 5].AutoFilter = true;
            lista.Cells[1, 1, numerWiersza - 1, 5].AutoFitColumns();
            lista.Column(1).Width = 25;
            lista.Column(2).Width = 35;
            lista.Column(3).Style.Numberformat.Format = "#,##0.00 zł";
            lista.Column(3).Width = 15;
            lista.Column(4).Width = 20;
            lista.Column(5).Width = 18;

            lista.View.FreezePanes(2, 1);

            // Pobranie pliku
            var plik = package.GetAsByteArray();

            return File(
                plik,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"Skarbnik_Wydatki_{DateTime.UtcNow.ToString("yyyyMMdd")}.xlsx");
        }
        public IActionResult OnGetTestExcel2()
        {
            using var package = new ExcelPackage();

            var arkusz = package.Workbook.Worksheets.Add("Test");

            arkusz.Cells["A1"].Value = "Uczeń";
            arkusz.Cells["B1"].Value = "Kwota";
            arkusz.Cells["C1"].Value = "Status";

            arkusz.Cells["A2"].Value = "Jan Kowalski";
            arkusz.Cells["B2"].Value = 50.00;
            arkusz.Cells["C2"].Value = "Nie zapłacono";

            // Formatowanie nagłówka
            using (var zakres = arkusz.Cells["A1:C1"])
            {
                zakres.Style.Font.Bold = true;
                zakres.Style.Fill.PatternType = ExcelFillStyle.Solid;
                zakres.Style.Fill.BackgroundColor.SetColor(
                    System.Drawing.Color.LightBlue);
            }

            arkusz.Cells["B2"].Style.Numberformat.Format = "#,##0.00 zł";
            arkusz.Cells["A1:C2"].AutoFitColumns();

            var plik = package.GetAsByteArray();

            return File(
                plik,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "TestEPPlus.xlsx");
        }
        public PodsumowanieVM Podsumowanie { get; set; } = new();

        public async Task OnGetAsync()
        {
            var uczniowie = await _context.Uczniowie
                .AsNoTracking()
                .OrderBy(u => u.Id)
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
