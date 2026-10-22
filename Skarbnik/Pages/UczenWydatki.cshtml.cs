using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Skarbnik.DB;
using Skarbnik.Services;
using Skarbnik.VM;

namespace Skarbnik.Pages
{
    public class UczenWydatkiModel : PageModel
    {
        private readonly UczenService _uczenService;
        public int UczenId { get; set; }
        public UczenWydatkiModel(UczenService uczenService)
        {
            _uczenService = uczenService;
        }

        public Uczen UczenDane { get; set; } = null!;
        public List<IdName> WydatkiNieprzypisane { get; set; } = new();

        public List<UczenWydatek> Wydatki { get; set; } = new();

        public decimal SumaWszystkich { get; set; }

        public decimal SumaNiezaplaconych { get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            UczenId = id;
            var uczen = await _uczenService.PobierzUczniaAsync(id, tylkoDoOdczytu: false);

            if (uczen == null)
                return NotFound();

            UczenDane = uczen;

            Wydatki = await _uczenService.PobierzWydatkiUczniaAsync(id);

            SumaWszystkich = Wydatki
                .Sum(uw => uw.Wydatek.Kwota);
            SumaNiezaplaconych = Wydatki
                .Where(uw => !uw.CzyZaplacono)
                .Sum(uw => uw.Wydatek.Kwota);

            WydatkiNieprzypisane = await _uczenService.PobierzNieprzypisaneWydatkiAsync(id);
            return Page();
        }
    }
}
