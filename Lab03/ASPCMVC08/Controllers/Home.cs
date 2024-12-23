using Microsoft.AspNetCore.Mvc;

namespace ASPCMVC08.Controllers
{
    public class Home : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}