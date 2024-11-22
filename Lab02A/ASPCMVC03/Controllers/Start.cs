using Microsoft.AspNetCore.Mvc;
using ASPCMVC03.Models;
using System.Diagnostics;


namespace ASPCMVC03.Controllers
{
    public class Start : Controller
    {


        private readonly ILogger<Start> _logger;

        public Start(ILogger<Start> logger)
        {
            _logger = logger;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult One()
        {
            return View();
        }


        public IActionResult Two()
        {
            return View();
        }


        public IActionResult Three()
        {
            return View();
        }

        public IActionResult BELSTU()
        {
            return Redirect("https://belstu.by");
        }

        public IActionResult Error404()
        {
            return Redirect("http://localhost:5288/Start/713271");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
