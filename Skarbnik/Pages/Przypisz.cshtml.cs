using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Skarbnik.DB;

namespace Skarbnik.Pages
{
    public class PrzypiszModel : PageModel
    {
        private readonly AppDbContext _context;

        public PrzypiszModel(AppDbContext context)
        {
            _context = context;
        }

        public Uczen Uczen { get; set; } = null!;

        public Wydatek Wydatek { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync(int uczenId, int wydatekId)
        {
            var dane = await PobierzDaneAsync(uczenId, wydatekId);

            if (dane == null)
                return NotFound();

            Uczen = dane.Value.Uczen;
            Wydatek = dane.Value.Wydatek;

            return Page();
        }
        public async Task<IActionResult> OnPostAsync(int uczenId, int wydatekId,    bool czyZaplacono)
        {
            var uczen = await _context.Uczniowie.FirstOrDefaultAsync(u => u.Id == uczenId);

            var wydatek = await _context.Wydatki.FirstOrDefaultAsync(w => w.Id == wydatekId);

            if (uczen == null || wydatek == null)
                return NotFound();

            // SprawdŸ, czy przypisanie ju¿ istnieje.
            var istnieje = await _context.UczenWydatki.AnyAsync(uw =>uw.UczenId == uczenId && uw.WydatekId == wydatekId);

            if (istnieje)
            {
                return RedirectToPage("/Index");
            }

            var przypisanie = new UczenWydatek
            {
                UczenId = uczenId,
                WydatekId = wydatekId,
                CzyZaplacono = czyZaplacono,
                DataZaplaty = czyZaplacono
                    ? DateTime.UtcNow
                    : null
            };

            _context.UczenWydatki.Add(przypisanie);

            await _context.SaveChangesAsync();

            return RedirectToPage("/Index");
        }
        private async Task<(Uczen Uczen, Wydatek Wydatek)?> PobierzDaneAsync(
       int uczenId,
       int wydatekId)
        {
            var uczen = await _context.Uczniowie
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == uczenId);

            var wydatek = await _context.Wydatki
                .AsNoTracking()
                .FirstOrDefaultAsync(w => w.Id == wydatekId);

            if (uczen == null || wydatek == null)
                return null;

            return (uczen, wydatek);
        }

    }
}
