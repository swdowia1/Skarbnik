using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Skarbnik.DB;

namespace Skarbnik.Pages
{
    public class DodajWydatekModel : PageModel
    {
        private readonly AppDbContext _context; 
        [BindProperty]
        public string Nazwa { get; set; } = string.Empty;

        [BindProperty]
        public decimal Kwota { get; set; }
        public DodajWydatekModel(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrWhiteSpace(Nazwa))
            {
                ModelState.AddModelError(
                    nameof(Nazwa),
                    "Podaj nazwê wydatku.");
            }

            if (Kwota <= 0)
            {
                ModelState.AddModelError(
                    nameof(Kwota),
                    "Kwota musi byæ wiêksza od zera.");
            }

            if (!ModelState.IsValid)
            {
             
                return Page();
            }

            var uczniowie = await _context.Uczniowie
                .AsNoTracking()
                .Select(u => u.Id)
                .ToListAsync();

            var wydatek = new Wydatek
            {
                Nazwa = Nazwa.Trim(),
                Kwota = Kwota
            };

            _context.Wydatki.Add(wydatek);

            foreach (var uczenId in uczniowie)
            {
                _context.UczenWydatki.Add(new UczenWydatek
                {
                    UczenId = uczenId,
                    Wydatek = wydatek,
                    CzyZaplacono = false,
                    DataZaplaty = null
                });
            }

            await _context.SaveChangesAsync();

            return RedirectToPage("/Index");
        }
        }
}
