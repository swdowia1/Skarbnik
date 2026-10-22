using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Skarbnik.DB;
using Skarbnik.Extensions;
using Skarbnik.Services;

namespace Skarbnik.Pages
{
    public class SzczegolyModel : PageModel
    {
        private readonly UczenService _uczenService;
        public UczenWydatek Przypisanie { get; set; } = null!;
        public SzczegolyModel(UczenService uczenService)
        {
            _uczenService = uczenService;
        }
        public async Task<IActionResult> OnPostZaplaconoAsync(int id)
        {
            var zapisano = await _uczenService.UstawCzyZaplaconoAsync(id, true);
            if (!zapisano)
                return NotFound();

            return RedirectToPage(new { id });
        }
        
        public async Task<IActionResult> OnPostNiezaplaconeAsync(int id)
        {
            if(!User.IsLoggedIn())
    return RedirectToPage("/Login");
            var zapisano = await _uczenService.UstawCzyZaplaconoAsync(id, false);
            if (!zapisano)
                return NotFound();

            return RedirectToPage(new { id });
        }

        public async Task<IActionResult> OnPostUsunAsync(int id)
        {
            var usunieto = await _uczenService.UsunPrzypisanieAsync(id);
            if (!usunieto)
                return NotFound();

            return RedirectToPage("/Index");
        }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            var przypisanie = await _uczenService.PobierzPrzypisanieAsync(id);

            if (przypisanie == null)
                return NotFound();

            Przypisanie = przypisanie;

            return Page();
        }

    }
}
