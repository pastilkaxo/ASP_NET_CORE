using Microsoft.AspNetCore.Mvc;

namespace Lab03.Controllers
{
    public class Calc : Controller
    {
        [HttpGet]
        public IActionResult Index(string? press,string? x,string? y)
        {

            float xValue, yValue;
            bool xValid = float.TryParse(x, out xValue);
            bool yValid = float.TryParse(y, out yValue);

            ViewBag.x = xValue; ViewBag.y = yValue;
            ViewBag.z = null;
            ViewBag.press = press;

            if ((!xValid || !yValid) && (!string.IsNullOrEmpty(x) || !string.IsNullOrEmpty(y)))
            {
                ViewBag.z = null;
                ViewBag.Error = "Неверный тип данных!";
            }

            return View("Calc");
        }

        [HttpPost]
        public IActionResult Sum(string x , string y)
        {
            float xValue, yValue;
            bool xValid = float.TryParse(x, out xValue);
            bool yValid = float.TryParse(y, out yValue);


            ViewBag.x = xValue;
            ViewBag.y = yValue;
            ViewBag.z = xValue + yValue;
            ViewBag.press = "+";

            if ((!xValid || !yValid) && (!string.IsNullOrEmpty(x) || !string.IsNullOrEmpty(y)))
            {
                ViewBag.z = null;
                ViewBag.Error = "Неверный тип данных!";
            }


            return View("Calc");
        }
        [HttpPost]
        public IActionResult Sub(string x, string y)
        {
            float xValue, yValue;
            bool xValid = float.TryParse(x, out xValue);
            bool yValid = float.TryParse(y, out yValue);


            ViewBag.x = xValue;
            ViewBag.y = yValue;
            ViewBag.z = xValue - yValue;
            ViewBag.press = "-";

            if ((!xValid || !yValid) && (!string.IsNullOrEmpty(x) || !string.IsNullOrEmpty(y)))
            {
                ViewBag.z = null;
                ViewBag.Error = "Неверный тип данных!";
            }


            return View("Calc");
        }

        [HttpPost]
        public IActionResult Mul(string x, string y)
        {
            float xValue, yValue;
            bool xValid = float.TryParse(x, out xValue);
            bool yValid = float.TryParse(y, out yValue);


            ViewBag.x = xValue;
            ViewBag.y = yValue;
            ViewBag.z = xValue * yValue;
            ViewBag.press = "*";

            if ((!xValid || !yValid) && (!string.IsNullOrEmpty(x) || !string.IsNullOrEmpty(y)))
            {
                ViewBag.z = null;
                ViewBag.Error = "Неверный тип данных!";
            }


            return View("Calc");
        }
        [HttpPost]
        public IActionResult Div(string x, string y)
        {
            float xValue, yValue;
            bool xValid = float.TryParse(x, out xValue);
            bool yValid = float.TryParse(y, out yValue);


            ViewBag.x = xValue;
            ViewBag.y = yValue;
            ViewBag.z = xValue / yValue;
            ViewBag.press = "/";

            if ((!xValid || !yValid ) && (!string.IsNullOrEmpty(x) || !string.IsNullOrEmpty(y)))
            {
                ViewBag.z = null;
                ViewBag.Error = "Неверный тип данных!";
            }
            else if (yValue == 0 )
            {
                ViewBag.z = (float)0.0;
                ViewBag.Error = "Деление на 0!";
            }




            return View("Calc");
        }
    }
}
