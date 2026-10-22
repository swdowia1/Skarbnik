using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Skarbnik.DB;
using Skarbnik.Services;
using Skarbnik.VM;

namespace Skarbnik.Pages
{
    public class WydatekModel : PageModel
    {
        private readonly UczenService _uczenService;
        public Wydatek WydatekDane { get; set; } = null!;

        public List<UczenStatusVM> Uczniowie { get; set; } = new();
        public WydatekModel(UczenService uczenService)
        {
            _uczenService = uczenService;
        }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            var wydatek = await _uczenService.PobierzWydatekAsync(id);
            if (wydatek == null)
                return NotFound();
            WydatekDane = wydatek;

            Uczniowie = await _uczenService.PobierzStatusyUczniowWydatkuAsync(id);

            return Page();
        }
    }
}
