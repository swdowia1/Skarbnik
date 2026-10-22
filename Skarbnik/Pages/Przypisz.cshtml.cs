using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Skarbnik.DB;
using Skarbnik.Services;

namespace Skarbnik.Pages
{
    public class PrzypiszModel : PageModel
    {
        private readonly UczenService _uczenService;

        public PrzypiszModel(UczenService uczenService)
        {
            _uczenService = uczenService;
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
            var uczen = await _uczenService.PobierzUczniaAsync(uczenId);
            var wydatek = await _uczenService.PobierzWydatekAsync(wydatekId);

            if (uczen == null || wydatek == null)
                return NotFound();

            await _uczenService.PrzypiszWydatekAsync(uczenId, wydatekId, czyZaplacono);

            return RedirectToPage("/Index");
        }
        private async Task<(Uczen Uczen, Wydatek Wydatek)?> PobierzDaneAsync(
       int uczenId,
       int wydatekId)
        {
            var uczen = await _uczenService.PobierzUczniaAsync(uczenId);
            var wydatek = await _uczenService.PobierzWydatekAsync(wydatekId);

            if (uczen == null || wydatek == null)
                return null;

            return (uczen, wydatek);
        }

    }
}
