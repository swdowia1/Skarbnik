using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Skarbnik.DB;
using Skarbnik.VM;

namespace Skarbnik.Pages
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext context)
        {
            _context = context;
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
                .OrderBy(w => w.Nazwa)
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
                            CzyZaplacono = przypisanie.CzyZaplacono
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
                            WydatekId = wydatek.Id
                        });
                    }
                }

                Podsumowanie.Uczniowie.Add(wiersz);
            }
        }
    }
}
