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


        //dodawanie dlawszystkich uczniów do wydatku
        // `/api/baza/wydatek/${wydatekId}/addall`,
        [HttpPost("wydatek/{wydatekId:int}/addall")]
        public async Task<IActionResult> DodajWszystkichUczniowDoWydatku(int wydatekId)
        {
            var uczniowie = await _context.Uczniowie.ToListAsync();
            var przypisania = new List<UczenWydatek>();
            foreach (var uczen in uczniowie)
            {
                var istniejePrzypisanie = await _context.UczenWydatki
                    .AnyAsync(x => x.WydatekId == wydatekId && x.UczenId == uczen.Id);
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

        // Oznaczenie wpłaty jako zapłaconej
        [HttpPost("wydatek/{wydatekId:int}/uczen/{uczenId:int}/zaplacono")]
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



        // Cofnięcie potwierdzenia wpłaty
        [HttpPost("wydatek/{wydatekId:int}/uczen/{uczenId:int}/usun")]
        public async Task<IActionResult> UsunWpate(
            int wydatekId,
            int uczenId)
        {
            var przypisanie = await _context.UczenWydatki
                .FirstOrDefaultAsync(x =>
                    x.WydatekId == wydatekId &&
                    x.UczenId == uczenId);
            if(przypisanie == null)
            {
                return Ok(new
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
        // /api/baza/wydatek/${wydatekId}/usun`
        [HttpPost("wydatek/{wydatekId:int}/usun")]
        public async Task<IActionResult> UsunWydatek(int wydatekId )
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
        
            //`/api/baza/wydatek/${wydatekId}/uczen/${uczenId}/zmianastatus`,
            [HttpPost("wydatek/{wydatekId:int}/uczen/{uczenId:int}/zmianastatus")]
        public async Task<IActionResult> zmianastatus(
           int wydatekId,
           int uczenId)
        {
            var przypisanie = await _context.UczenWydatki
                .FirstOrDefaultAsync(x =>
                    x.WydatekId == wydatekId &&
                    x.UczenId == uczenId);
            if (przypisanie == null)
            {
              UczenWydatek uw=new UczenWydatek();
                uw.WydatekId=wydatekId;
                uw.UczenId=uczenId;
                uw.CzyZaplacono=false;
                _context.UczenWydatki.Add(uw);
                _context.SaveChanges();

                return Ok(new
                {
                    message = "Dodano przypisanie ucznia do wydatku."
                });
            }
            if (przypisanie.CzyZaplacono == false)
            {
                przypisanie.CzyZaplacono = true;
                przypisanie.DataZaplaty = DateTime.UtcNow;
                _context.SaveChanges();
                return Ok(new
                {
                    message = "Oznaczono jako zapłacone."
                });
            }
            else
            {
                przypisanie.CzyZaplacono = false;
                przypisanie.DataZaplaty = null;
                _context.SaveChanges();
                return Ok(new
                {
                    message = "Oznaczono jako niezapłacone."
                });
            }
           
        }
    }
}
