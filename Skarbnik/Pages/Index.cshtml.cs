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
        public IActionResult OnGetTestExcel()
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
