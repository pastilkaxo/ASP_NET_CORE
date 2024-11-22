using Microsoft.AspNetCore.Mvc;

namespace ASPCMVC04.Controllers
{
    public class Status : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult S200()
        {
            int rand = new Random().Next(200, 299);
            return StatusCode(rand, $"[{rand}] Запрос успешно выполнен!");
            // return Ok("[200]:Запрос успешно выполнен!");
        }


    
        public IActionResult S300() { 
            int rand = new Random().Next(300,399);
            return StatusCode(rand, $"[{rand}]");
        }

        public IActionResult S500()
        {
                try
                {
                    int x = 5;
                    int y = 0;
                    int res = x / y;
                }
                catch (DivideByZeroException)
                {
                    int rand = new Random().Next(500, 599);
                    return StatusCode(rand, $"[{rand}] Деление на ноль!");
                }
                return Ok();

        }

    }
}
