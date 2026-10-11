using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Skarbnik.DB;
using Skarbnik.VM;

namespace Skarbnik.Pages
{
    public class UczenModel : PageModel
    {
        private readonly AppDbContext _context;

        public UczenModel(AppDbContext context)
        {
            _context = context;
        }
        public IList<UczenVM> Uczniowie { get; set; } = new List<UczenVM>();
        public decimal SumaWszystkichZaleglosci { get; set; }
        public async Task OnGetAsync()
        {
            Uczniowie = await _context.Uczniowie
        .AsNoTracking()
        .OrderBy(u => u.Id)
        .Select(u => new UczenVM
        {
            Id = u.Id,
            Nazwisko = u.Nazwisko,

            Suma = u.UczenWydatki
                .Where(uw => !uw.CzyZaplacono)
                .Sum(uw => (decimal?)uw.Wydatek.Kwota) ?? 0,

            Liczba = u.UczenWydatki
                .Count(uw => !uw.CzyZaplacono)
        })
        .ToListAsync();
            SumaWszystkichZaleglosci = Uczniowie.Sum(u => u.Suma);
        }
    }
}
