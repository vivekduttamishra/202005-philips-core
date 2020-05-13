using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HelloMVC.Controllers
{
    public class MathsController : Controller
    {
        public int Plus(int x,int y)
        {
            return x + y;
        }

        public int Minus(int x, int y)
        {
            return x - y;
        }

        public int Multiply(int x, int y)
        {
            return x * y;
        }

        public ActionResult Divide(int x, int y)
        {
            if (y == 0)
            {
                Response.StatusCode = 400;
                return View("ErrorView", (object)"Can't divide by Zero");
            }
            return Content(string.Format("{0}", x / y)); //good result
        }

        public int Factorial(int x)
        {
            int fx = 1;
            while (x > 1)
                fx *= x--;
            return fx;
        }
    }
}