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
