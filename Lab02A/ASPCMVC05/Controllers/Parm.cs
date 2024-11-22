using Microsoft.AspNetCore.Mvc;

namespace ASPCMVC05.Controllers
{
    public class Parm : Controller
    {
        public IActionResult Index(string Id)
        {
            ViewBag.Id = Id;
            return View();
        }

        public IActionResult Uri01(int Id)
        {
            ViewBag.Id = Id;
            return View();
        }


        public IActionResult Uri02(int? Id)
        {
            ViewBag.Id = Id;
            return View();
        }

        public IActionResult Uri03(float Id)
        {
            ViewBag.Id = Id;
            return View();
        }

        public IActionResult Uri04(DateTime Id)
        {
            ViewBag.Id = Id;
            return View();
        }
    }
}
