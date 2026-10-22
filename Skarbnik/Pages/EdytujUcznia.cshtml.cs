using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Skarbnik.DB;
using Skarbnik.Services;
using System.ComponentModel.DataAnnotations;

namespace Skarbnik.Pages
{
    [Authorize]
    public class EdytujUczniaModel : PageModel
    {
        private readonly UczenService _uczenService;

        [BindProperty]
        public int Id { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Podaj nazwisko ucznia.")]
        [StringLength(100, ErrorMessage = "Nazwisko może mieć maksymalnie 100 znaków.")]
        [Display(Name = "Nazwisko")]
        public string Nazwisko { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "Podaj skrót ucznia.")]
        [StringLength(20, ErrorMessage = "Skrót może mieć maksymalnie 20 znaków.")]
        [Display(Name = "Skrót")]
        public string Skrot { get; set; } = string.Empty;

        public EdytujUczniaModel(UczenService uczenService)
        {
            _uczenService = uczenService;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var uczen = await _uczenService.PobierzPoIdAsync(id);
            if (uczen == null)
                return NotFound();

            Id = uczen.Id;
            Nazwisko = uczen.Nazwisko;
            Skrot = uczen.Skrot;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Nazwisko = Nazwisko?.Trim() ?? string.Empty;
            Skrot = Skrot?.Trim() ?? string.Empty;

            if (Id <= 0)
                return BadRequest();

            if (!ModelState.IsValid)
                return Page();

            var zapisano = await _uczenService.AktualizujAsync(new Uczen
            {
                Id = Id,
                Nazwisko = Nazwisko,
                Skrot = Skrot
            });

            if (!zapisano)
                return NotFound();

            return RedirectToPage("/UczenWydatki", new { id = Id });
        }
    }
}
