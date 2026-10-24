using Microsoft.AspNetCore.Mvc.RazorPages;
using Skarbnik.Services;
using Skarbnik.VM;

namespace Skarbnik.Pages
{
    public class DashboardModel : PageModel
    {
        private readonly UczenService _uczenService;

        public DashboardModel(UczenService uczenService)
        {
            _uczenService = uczenService;
        }

        public decimal SumaNaleznosci { get; private set; }

        public decimal SumaWplacona { get; private set; }

        public int LiczbaUczniowZZaleglosciami { get; private set; }

        public List<DashboardActivityViewModel> OstatnieWydatki { get; private set; } = new();

        public List<DashboardActivityViewModel> OstatnieWplaty { get; private set; } = new();

        public async Task OnGetAsync()
        {
            var uczniowie = await _uczenService.PobierzWszystkichAsync();
            var wydatki = await _uczenService.PobierzWszystkieWydatkiAsync();
            var przypisania = await _uczenService.PobierzWszystkiePrzypisaniaAsync();

            SumaNaleznosci = przypisania.Sum(p => p.Wydatek.Kwota);
            SumaWplacona = przypisania
                .Where(p => p.CzyZaplacono)
                .Sum(p => p.Wydatek.Kwota);
            LiczbaUczniowZZaleglosciami = przypisania
                .Where(p => !p.CzyZaplacono)
                .Select(p => p.UczenId)
                .Distinct()
                .Count();

            OstatnieWydatki = wydatki
                .OrderByDescending(w => w.CreatedAt)
                .Take(5)
                .Select(w => new DashboardActivityViewModel
                {
                    Nazwa = w.Nazwa,
                    Kwota = w.Kwota,
                    Data = w.CreatedAt
                })
                .ToList();

            var uczniowiePoId = uczniowie.ToDictionary(u => u.Id, u => u.Nazwisko);
            OstatnieWplaty = przypisania
                .Where(p => p.CzyZaplacono)
                .OrderByDescending(p => p.DataZaplaty ?? p.CreatedAt)
                .Take(5)
                .Select(p => new DashboardActivityViewModel
                {
                    Nazwa = p.Wydatek.Nazwa,
                    Uczen = uczniowiePoId.GetValueOrDefault(p.UczenId, string.Empty),
                    Kwota = p.Wydatek.Kwota,
                    Data = p.DataZaplaty ?? p.CreatedAt
                })
                .ToList();
        }
    }
}
