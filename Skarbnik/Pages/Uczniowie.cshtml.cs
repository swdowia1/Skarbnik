using Microsoft.AspNetCore.Mvc.RazorPages;
using Skarbnik.DB;
using Skarbnik.Services;

namespace Skarbnik.Pages
{
    public class UczniowieModel : PageModel
    {
        private readonly UczenService _uczenService;

        public List<Uczen> Uczniowie { get; set; } = new();

        public UczniowieModel(UczenService uczenService)
        {
            _uczenService = uczenService;
        }

        public async Task OnGetAsync()
        {
            Uczniowie = await _uczenService.PobierzWszystkichAsync();
        }
    }
}
