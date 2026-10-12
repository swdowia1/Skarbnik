using Microsoft.AspNetCore.Mvc;
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

       
    }
}
