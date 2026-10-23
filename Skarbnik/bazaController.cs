using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Skarbnik.Services;

namespace Skarbnik
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class bazaController : ControllerBase
    {
        private readonly UczenService _uczenService;

        public bazaController(UczenService uczenService)
        {
            _uczenService = uczenService;
        }

        [HttpPost("wydatek/{wydatekId:int}/addall")]
        public async Task<IActionResult> DodajWszystkichUczniowDoWydatku(int wydatekId)
        {
            var liczbaDodanych = await _uczenService.PrzypiszWszystkichUczniowAsync(wydatekId);
            if (liczbaDodanych == null)
            {
                return NotFound(new { message = "Nie znaleziono wydatku." });
            }

            return Ok(new
            {
                message = "Dodano wszystkich uczniów do wydatku.",
                liczbaDodanych
            });
        }

        [HttpPost("wydatek/{wydatekId:int}/uczen/{uczenId:int}/status")]
        public async Task<IActionResult> OznaczJakoZaplacono(int wydatekId, int uczenId)
        {
            var przypisanie = await _uczenService.PobierzPrzypisanieDlaUczniaIWydatkuAsync(uczenId, wydatekId);
            if (przypisanie == null)
            {
                return NotFound(new { message = "Uczeń nie jest przypisany do tego wydatku." });
            }

            await _uczenService.UstawCzyZaplaconoAsync(uczenId, wydatekId, true);
            var zaktualizowanePrzypisanie = await _uczenService.PobierzPrzypisanieDlaUczniaIWydatkuAsync(uczenId, wydatekId);

            return Ok(new
            {
                message = "Wpłatę oznaczono jako zapłaconą.",
                dataZaplaty = zaktualizowanePrzypisanie!.DataZaplaty
            });
        }

        [HttpPost("wydatek/{wydatekId:int}/uczen/{uczenId:int}/usun")]
        public async Task<IActionResult> UsunWplate(int wydatekId, int uczenId)
        {
            var przypisanie = await _uczenService.PobierzPrzypisanieDlaUczniaIWydatkuAsync(uczenId, wydatekId);
            if (przypisanie == null)
            {
                return NotFound(new { message = "Nie znaleziono przypisania ucznia do wydatku." });
            }

            await _uczenService.UsunPrzypisanieAsync(przypisanie.Id);
            return Ok(new { message = "Usunięto przypisanie ucznia do wydatku." });
        }

        [HttpPost("wydatek/{wydatekId:int}/usun")]
        public async Task<IActionResult> UsunWydatek(int wydatekId)
        {
            if (!await _uczenService.UsunWydatekAsync(wydatekId))
            {
                return NotFound(new { message = "Nie znaleziono wydatku." });
            }

            return Ok(new { message = "Wydatek został usunięty." });
        }

        [HttpPost("wydatek/{wydatekId:int}/uczen/{uczenId:int}/zmianastatus")]
        public async Task<IActionResult> ZmienStatus(int wydatekId, int uczenId)
        {
            //Thread.Sleep(6_000); // Dodanie opóźnienia 1 sekundy
            if (await _uczenService.PobierzWydatekAsync(wydatekId) == null)
            {
                return NotFound(new { message = "Nie znaleziono wydatku." });
            }

            if (await _uczenService.PobierzPoIdAsync(uczenId) == null)
            {
                return NotFound(new { message = "Nie znaleziono ucznia." });
            }

            var przypisanie = await _uczenService.PobierzPrzypisanieDlaUczniaIWydatkuAsync(uczenId, wydatekId);
            if (przypisanie == null)
            {
                await _uczenService.PrzypiszWydatekAsync(uczenId, wydatekId);
                return Ok(new { message = "Dodano przypisanie ucznia do wydatku." });
            }

            var czyZaplacono = !przypisanie.CzyZaplacono;
            await _uczenService.UstawCzyZaplaconoAsync(uczenId, wydatekId, czyZaplacono);

            return Ok(new
            {
                message = czyZaplacono
                    ? "Oznaczono jako zapłacone."
                    : "Oznaczono jako niezapłacone."
            });
        }
    }
}
