using Microsoft.AspNetCore.Mvc;

namespace ASPCMVC08.Controllers
{
    public class Admin : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
