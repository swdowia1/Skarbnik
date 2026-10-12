using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Skarbnik.DB;

namespace Skarbnik.Pages
{
    public class SzczegolyModel : PageModel
    {
        private readonly AppDbContext _context;
        public UczenWydatek Przypisanie { get; set; } = null!;
        public SzczegolyModel(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> OnPostZaplaconoAsync(int id)
        {
            var przypisanie = await _context.UczenWydatki
                .FirstOrDefaultAsync(uw => uw.Id == id);

            if (przypisanie == null)
                return NotFound();

            przypisanie.CzyZaplacono = true;
            przypisanie.DataZaplaty = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return RedirectToPage(new { id });
        }

        public async Task<IActionResult> OnPostNiezaplaconeAsync(int id)
        {
            var przypisanie = await _context.UczenWydatki
                .FirstOrDefaultAsync(uw => uw.Id == id);

            if (przypisanie == null)
                return NotFound();

            przypisanie.CzyZaplacono = false;
            przypisanie.DataZaplaty = null;

            await _context.SaveChangesAsync();

            return RedirectToPage(new { id });
        }

        public async Task<IActionResult> OnPostUsunAsync(int id)
        {
            var przypisanie = await _context.UczenWydatki
                .FirstOrDefaultAsync(uw => uw.Id == id);

            if (przypisanie == null)
                return NotFound();

            _context.UczenWydatki.Remove(przypisanie);

            await _context.SaveChangesAsync();

            return RedirectToPage("/Index");
        }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            var przypisanie = await _context.UczenWydatki
                .AsNoTracking()
                .Include(uw => uw.Uczen)
                .Include(uw => uw.Wydatek)
                .FirstOrDefaultAsync(uw => uw.Id == id);

            if (przypisanie == null)
                return NotFound();

            Przypisanie = przypisanie;

            return Page();
        }

    }
}
