using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Skarbnik.DB;
using Skarbnik.VM;

namespace Skarbnik
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class bazaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public bazaController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("wydatek/{wydatekId:int}/addall")]
        public async Task<IActionResult> DodajWszystkichUczniowDoWydatku(int wydatekId)
        {
            var wydatekIstnieje = await _context.Wydatki
                .AnyAsync(w => w.Id == wydatekId);

            if (!wydatekIstnieje)
            {
                return NotFound(new
                {
                    message = "Nie znaleziono wydatku."
                });
            }

            var uczniowie = await _context.Uczniowie.ToListAsync();
            var przypisania = new List<UczenWydatek>();

            foreach (var uczen in uczniowie)
            {
                var istniejePrzypisanie = await _context.UczenWydatki
                    .AnyAsync(x =>
                        x.WydatekId == wydatekId &&
                        x.UczenId == uczen.Id);

                if (!istniejePrzypisanie)
                {
                    przypisania.Add(new UczenWydatek
                    {
                        WydatekId = wydatekId,
                        UczenId = uczen.Id,
                        CzyZaplacono = false
                    });
                }
            }

            if (przypisania.Count > 0)
            {
                _context.UczenWydatki.AddRange(przypisania);
                await _context.SaveChangesAsync();
            }

            return Ok(new
            {
                message = "Dodano wszystkich uczniów do wydatku.",
                liczbaDodanych = przypisania.Count
            });
        }

        [HttpPost("wydatek/{wydatekId:int}/uczen/{uczenId:int}/status")]
        public async Task<IActionResult> OznaczJakoZaplacono(
            int wydatekId,
            int uczenId)
        {
            var przypisanie = await _context.UczenWydatki
                .FirstOrDefaultAsync(x =>
                    x.WydatekId == wydatekId &&
                    x.UczenId == uczenId);

            if (przypisanie == null)
            {
                return NotFound(new
                {
                    message = "Uczeń nie jest przypisany do tego wydatku."
                });
            }

            przypisanie.CzyZaplacono = true;
            przypisanie.DataZaplaty = DateTime.Now;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Wpłatę oznaczono jako zapłaconą.",
                dataZaplaty = przypisanie.DataZaplaty
            });
        }

        [HttpPost("wydatek/{wydatekId:int}/uczen/{uczenId:int}/usun")]
        public async Task<IActionResult> UsunWplate(
            int wydatekId,
            int uczenId)
        {
            var przypisanie = await _context.UczenWydatki
                .FirstOrDefaultAsync(x =>
                    x.WydatekId == wydatekId &&
                    x.UczenId == uczenId);

            if (przypisanie == null)
            {
                return NotFound(new
                {
                    message = "Nie znaleziono przypisania ucznia do wydatku."
                });
            }

            _context.UczenWydatki.Remove(przypisanie);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Usunięto przypisanie ucznia do wydatku."
            });
        }

        [HttpPost("wydatek/{wydatekId:int}/usun")]
        public async Task<IActionResult> UsunWydatek(int wydatekId)
        {
            var wydatek = await _context.Wydatki
                .FirstOrDefaultAsync(w => w.Id == wydatekId);

            if (wydatek == null)
            {
                return NotFound(new
                {
                    message = "Nie znaleziono wydatku."
                });
            }

            var przypisania = await _context.UczenWydatki
                .Where(uw => uw.WydatekId == wydatekId)
                .ToListAsync();

            _context.UczenWydatki.RemoveRange(przypisania);
            _context.Wydatki.Remove(wydatek);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Wydatek został usunięty."
            });
        }

        [HttpPost("wydatek/{wydatekId:int}/uczen/{uczenId:int}/zmianastatus")]
        public async Task<IActionResult> ZmienStatus(
            int wydatekId,
            int uczenId)
        {
            var wydatekIstnieje = await _context.Wydatki
                .AnyAsync(w => w.Id == wydatekId);

            if (!wydatekIstnieje)
            {
                return NotFound(new
                {
                    message = "Nie znaleziono wydatku."
                });
            }

            var uczenIstnieje = await _context.Uczniowie
                .AnyAsync(u => u.Id == uczenId);

            if (!uczenIstnieje)
            {
                return NotFound(new
                {
                    message = "Nie znaleziono ucznia."
                });
            }

            var przypisanie = await _context.UczenWydatki
                .FirstOrDefaultAsync(x =>
                    x.WydatekId == wydatekId &&
                    x.UczenId == uczenId);

            if (przypisanie == null)
            {
                przypisanie = new UczenWydatek
                {
                    WydatekId = wydatekId,
                    UczenId = uczenId,
                    CzyZaplacono = false
                };

                _context.UczenWydatki.Add(przypisanie);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = "Dodano przypisanie ucznia do wydatku."
                });
            }

            if (!przypisanie.CzyZaplacono)
            {
                przypisanie.CzyZaplacono = true;
                przypisanie.DataZaplaty = DateTime.Now;
            }
            else
            {
                przypisanie.CzyZaplacono = false;
                przypisanie.DataZaplaty = null;
            }

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = przypisanie.CzyZaplacono
                    ? "Oznaczono jako zapłacone."
                    : "Oznaczono jako niezapłacone."
            });
        }
    }
}
