using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Skarbnik.DB;
using Skarbnik.Services;
using System.ComponentModel.DataAnnotations;

namespace Skarbnik.Pages
{
    [Authorize]
    public class DodajWydatekModel : PageModel
    {
        private readonly UczenService _uczenService;

        [BindProperty]
        public int? Id { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Podaj nazwę wydatku.")]
        [StringLength(100, ErrorMessage = "Nazwa może mieć maksymalnie 100 znaków.")]
        [Display(Name = "Nazwa wydatku")]
        public string Nazwa { get; set; } = string.Empty;

        [BindProperty]
        [Range(typeof(decimal), "0,01", "999999999",
            ErrorMessage = "Kwota musi wynosić co najmniej 0,01 zł.")]
        [Display(Name = "Kwota (zł)")]
        public decimal Kwota { get; set; }

        public bool CzyEdycja => Id.HasValue;

        public DodajWydatekModel(UczenService uczenService)
        {
            _uczenService = uczenService;
        }

        // Wczytanie strony: dodawanie lub edycja
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (!id.HasValue)
                return Page();

            var wydatek = await _uczenService.PobierzWydatekAsync(id.Value);

            if (wydatek == null)
                return NotFound();

            Id = wydatek.Id;
            Nazwa = wydatek.Nazwa;
            Kwota = wydatek.Kwota;

            return Page();
        }

        // Dodawanie wydatku bez przypisywania uczniom
        public async Task<IActionResult> OnPostDodajAsync()
        {
            if (!WalidujDane())
                return Page();

            await _uczenService.DodajWydatekAsync(Nazwa, Kwota);

            return RedirectToPage("/Index");
        }

        // Dodawanie wydatku i przypisanie wszystkim uczniom
        public async Task<IActionResult> OnPostDodajDlaWszystkichAsync()
        {
            if (!WalidujDane())
                return Page();

            await _uczenService.DodajWydatekDlaWszystkichAsync(Nazwa, Kwota);

            return RedirectToPage("/Index");
        }

        // Zapis zmian istniejącego wydatku
        public async Task<IActionResult> OnPostZapiszEdycjeAsync()
        {
            if (!Id.HasValue)
                return BadRequest();

            if (!WalidujDane())
                return Page();

            var zapisano = await _uczenService.AktualizujWydatekAsync(Id.Value, Nazwa, Kwota);
            if (!zapisano)
                return NotFound();

            return RedirectToPage("/Wydatek", new { id = Id.Value });
        }

        private bool WalidujDane()
        {
            if (string.IsNullOrWhiteSpace(Nazwa))
            {
                ModelState.AddModelError(
                    nameof(Nazwa), "Podaj nazwę wydatku.");
            }

            if (Kwota <= 0)
            {
                ModelState.AddModelError(
                    nameof(Kwota), "Kwota musi być większa od zera.");
            }

            return ModelState.IsValid;
        }
    }
}
