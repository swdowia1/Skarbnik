using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Skarbnik.DB;
using Skarbnik.VM;

namespace Skarbnik
{
    [Route("api/[controller]")]
    //[Route("api/ang")]
    [ApiController]
    public class bazaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public bazaController(AppDbContext context)
        {
            _context = context;
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
                przypisanie.DataZaplaty = DateTime.Now;
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
