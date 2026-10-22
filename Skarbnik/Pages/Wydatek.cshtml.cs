using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Skarbnik.DB;
using Skarbnik.Enums;
using Skarbnik.VM;

namespace Skarbnik.Pages
{
    public class WydatekModel : PageModel
    {
        private readonly AppDbContext _context;
        public Wydatek WydatekDane { get; set; } = null!;

        public List<UczenStatusVM> Uczniowie { get; set; } = new();
        public WydatekModel(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            WydatekDane = await _context.Wydatki
        .Where(w => w.Id == id)      
        .FirstOrDefaultAsync();
            var uczniowie = await _context.Uczniowie
      .AsNoTracking()
      .OrderBy(u => u.Id)
   
      .Select(u => new
      {
          u.Id,
          u.Nazwisko
      })
      .ToListAsync();

           Uczniowie= await (
    from u in _context.Uczniowie.AsNoTracking()

    join uw in _context.UczenWydatki
        .AsNoTracking()
        .Where(x => x.WydatekId == id)
        on u.Id equals uw.UczenId into przypisaniaUcznia

    from przypisanie in przypisaniaUcznia.DefaultIfEmpty()

    orderby u.Id

    select new UczenStatusVM
    {
        UczenId = u.Id,
         Nazwisko= u.Nazwisko,
        UczenWydatekId = przypisanie == null
            ? 0
            : przypisanie.Id,
        StanPlatnosc = przypisanie == null
            ? StanPlatnosc.Brak
            : przypisanie.CzyZaplacono
                ? StanPlatnosc.Zaplacono
                : StanPlatnosc.DoZaplaty,

        DataZaplaty = przypisanie == null
            ? (DateTime?)null
            : przypisanie.DataZaplaty
    }
).ToListAsync();

            return Page();
        }
    }
}
