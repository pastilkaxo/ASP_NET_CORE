using Microsoft.AspNetCore.Mvc;

namespace ASPCMVC06.Controllers
{
    public class TMResearch : Controller
    {


        public IActionResult Index()
        {
            return View();
        }


        public IActionResult M01(string? id,string? str)
        {
            string m = this.HttpContext.Request.Method;
            string response = $"{m}:M01";
            return Content(response, "text/plain");
        }


        public IActionResult M02(string? str)
        {
            string m = this.HttpContext.Request.Method;
            string response = $"{m}:M02";
            return Content(response,"text/plain");
        }

        public IActionResult M03(string? str)
        {
            string m = this.HttpContext.Request.Method;
            string response = $"{m}:M03";
            return Content(response, "text/plain");
        }

        public IActionResult MXX()
        {
            string m = this.HttpContext.Request.Method;
            return Content($"{m}:MXX","text/plain");
        }


    }
}
