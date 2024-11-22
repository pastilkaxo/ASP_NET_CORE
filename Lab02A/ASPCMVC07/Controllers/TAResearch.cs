using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;

namespace ASPCMVC07.Controllers
{
    [Route("it")]
    public class TAResearch : Controller
    {
        public IActionResult Index()
        {
            return Content("INDEX","text/plain");
        }

        [HttpGet]
        [Route("{n:int}/{str}")]
        public IActionResult M04(int? n, string? str )
        {
            string m = this.HttpContext.Request.Method;
            string response = $"{m}:M04";
            response += $":/{n}/{str}";
            return Content(response, "text/plain");
        }


        [HttpGet]
        [HttpPost]
        [Route("{b:bool}/{letters:alpha}")]
        public IActionResult M05(bool b , string letters)
        {
            string m = this.HttpContext.Request.Method;
            string response = $"{m}:M05";
            response += $": /{b}/{letters}";
            return Content(response, "text/plain");

        }

        [HttpGet]
        [HttpDelete]
        [Route("{f:float}/{str:minlength(2):maxlength(5)}")]
        public IActionResult M06(float f , string str)
        {
            string m = this.HttpContext.Request.Method;
            string response = $"{m}:M06";
            response += $": /{f}/{str}";
            return Content(response, "text/plain");


        }


        [HttpPut]
        [Route("{letters:minlength(3):maxlength(4):alpha}/{n:int:range(100,200)}/")]
        public IActionResult M07(int? n, string? letters)
        {
            string m = this.HttpContext.Request.Method;
            string response = $"{m}:M07";
            response += $":/{letters}/{n}";
            return Content(response, "text/plain");
        }

        [HttpPost]
        [Route("{mail:regex(^[[\\w-\\.]]+@([[\\w-]]+\\.)+com$)}")]
        public IActionResult M08(string? mail)
        {
            string m = this.HttpContext.Request.Method;
            string response = $"{m}:M08";
            response += $"/{mail}";
            return Content(response, "text/plain");
        }




    }
}
