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

        [HttpPost("deletelevel")]
        public async Task<ActionResult<int>> deletelevel(AddUczenWydatekWM add)
        {
           
            return new JsonResult(1);
        }

        //// Czwarta metoda GET zwracająca listę Job
        //[HttpPost("addtask")]
        //public async Task<ActionResult<int>> addtask([FromBody] AddTask dane)
        //{



        //    return new JsonResult(1);
        //}
        //deleteTasktime
        [HttpPost("deleteTasktime")]
        public async Task<ActionResult<int>> deleteTasktime([FromBody] int val)
        {

            return new JsonResult(1);
        }
        // Czwarta metoda GET zwracająca listę Job
        [HttpPost("deleteTask")]
        public async Task<ActionResult<int>> deleteTask([FromBody] int val)
        {

            return new JsonResult(1);
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
        [HttpPost("wydatek/{wydatekId:int}/uczen/{uczenId:int}/cofnij-wplate")]
        public async Task<IActionResult> CofnijWplate(
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

            przypisanie.CzyZaplacono = false;
            przypisanie.DataZaplaty = null;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Cofnięto potwierdzenie wpłaty."
            });
        }

    }
}
