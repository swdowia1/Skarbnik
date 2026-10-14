using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Skarbnik.DB;
using Skarbnik.VM;

namespace Skarbnik.Pages
{
    public class UczenWydatkiModel : PageModel
    {
        private readonly AppDbContext _context;

        public UczenWydatkiModel(AppDbContext context)
        {
            _context = context;
        }

        public Uczen UczenDane { get; set; } = null!;
        public List<IdName> WydatkiNieprzypisane { get; set; } = new();

        public List<UczenWydatek> Wydatki { get; set; } = new();

        public decimal SumaWszystkich { get; set; }

        public decimal SumaNiezaplaconych { get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            var uczen = await _context.Uczniowie
                .FirstOrDefaultAsync(u => u.Id == id);

            if (uczen == null)
                return NotFound();

            UczenDane = uczen;

            Wydatki = await _context.UczenWydatki
                .Include(uw => uw.Wydatek)
                .Where(uw => uw.UczenId == id)
                .OrderBy(uw => uw.Wydatek.Nazwa)
                .ToListAsync();

            SumaWszystkich = Wydatki
                .Sum(uw => uw.Wydatek.Kwota);
            var przypisaneIds = Wydatki.Select(uw => uw.WydatekId).ToList();
            SumaNiezaplaconych = Wydatki
                .Where(uw => !uw.CzyZaplacono)
                .Sum(uw => uw.Wydatek.Kwota);

            WydatkiNieprzypisane = await _context.Wydatki
    .Where(w => !przypisaneIds.Contains(w.Id))
    .OrderBy(w => w.Nazwa)
    .Select(w => new IdName() {Id=w.Id,Name=w.Nazwa })
    .ToListAsync();
            return Page();
        }
    }
}
