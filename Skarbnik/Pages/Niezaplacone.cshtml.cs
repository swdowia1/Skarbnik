using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Skarbnik.DB;
using Skarbnik.VM;

namespace Skarbnik.Pages
{
    public class NiezaplaconeModel : PageModel
    {
        private readonly AppDbContext _context;

        public NiezaplaconeModel(AppDbContext context)
        {
            _context = context;
        }

        public List<PozycjaNiezaplacona> Pozycje { get; private set; } = new();

        public decimal SumaNiezaplaconych => Pozycje.Sum(p => p.Kwota);

        public async Task OnGetAsync()
        {
            Pozycje = await _context.UczenWydatki
                .AsNoTracking()
                .Where(uw => !uw.CzyZaplacono)
                .OrderBy(uw => uw.Uczen.Nazwisko)
                .ThenBy(uw => uw.Wydatek.Nazwa)
                .Select(uw => new PozycjaNiezaplacona
                {
                    UczenId = uw.UczenId,
                    Nazwisko = uw.Uczen.Nazwisko,
                    WydatekId = uw.WydatekId,
                    NazwaWydatku = uw.Wydatek.Nazwa,
                    Kwota = uw.Wydatek.Kwota
                })
                .ToListAsync();
        }

    }
}
