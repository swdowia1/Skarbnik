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
        public int? Id { get; set; }

        [BindProperty]
        public string Nazwa { get; set; } = string.Empty;

        [BindProperty]
        public decimal Kwota { get; set; }

        public bool CzyEdycja => Id.HasValue;

        public DodajWydatekModel(AppDbContext context)
        {
            _context = context;
        }

        // Wczytanie strony: dodawanie lub edycja
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (!id.HasValue)
                return Page();

            var wydatek = await _context.Wydatki
                .AsNoTracking()
                .FirstOrDefaultAsync(w => w.Id == id.Value);

            if (wydatek == null)
                return NotFound();

            Id = wydatek.Id;
            Nazwa = wydatek.Nazwa;
            Kwota = wydatek.Kwota;

            return Page();
        }

        // Dodawanie wydatku bez przypisywania uczniom
        public async Task<IActionResult> OnPostDodajAsync()
        {
            if (!WalidujDane())
                return Page();

            var wydatek = new Wydatek
            {
                Nazwa = Nazwa.Trim(),
                Kwota = Kwota
            };

            _context.Wydatki.Add(wydatek);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Index");
        }

        // Dodawanie wydatku i przypisanie wszystkim uczniom
        public async Task<IActionResult> OnPostDodajDlaWszystkichAsync()
        {
            if (!WalidujDane())
                return Page();

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

        // Zapis zmian istniej¹cego wydatku
        public async Task<IActionResult> OnPostZapiszEdycjeAsync()
        {
            if (!Id.HasValue)
                return BadRequest();

            if (!WalidujDane())
                return Page();

            var wydatek = await _context.Wydatki
                .FirstOrDefaultAsync(w => w.Id == Id.Value);

            if (wydatek == null)
                return NotFound();

            wydatek.Nazwa = Nazwa.Trim();
            wydatek.Kwota = Kwota;

            await _context.SaveChangesAsync();

            return RedirectToPage("/Wydatek", new { id = wydatek.Id });
        }

        private bool WalidujDane()
        {
            if (string.IsNullOrWhiteSpace(Nazwa))
            {
                ModelState.AddModelError(
                    nameof(Nazwa), "Podaj nazwê wydatku.");
            }

            if (Kwota <= 0)
            {
                ModelState.AddModelError(
                    nameof(Kwota), "Kwota musi byæ wiêksza od zera.");
            }

            return ModelState.IsValid;
        }
    }
}