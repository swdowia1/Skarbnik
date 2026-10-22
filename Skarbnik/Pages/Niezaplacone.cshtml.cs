using Microsoft.AspNetCore.Mvc.RazorPages;
using Skarbnik.Services;
using Skarbnik.VM;

namespace Skarbnik.Pages
{
    public class NiezaplaconeModel : PageModel
    {
        private readonly UczenService _uczenService;

        public NiezaplaconeModel(UczenService uczenService)
        {
            _uczenService = uczenService;
        }

        public List<UnpaidItemViewModel> Pozycje { get; private set; } = new();

        public decimal SumaNiezaplaconych => Pozycje.Sum(p => p.Kwota);

        public async Task OnGetAsync()
        {
            Pozycje = await _uczenService.PobierzNiezaplaconeAsync();
        }

    }
}
