using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HelloMVC.Controllers
{
    /// <summary>
    /// How is the url http://localhost:57586/Home/About Processed    /// 
    /// 1. First part of URL identifies Controller. 
    ///     * We need a class with same name as Controller and Controller suffix (HomeController)
    ///     * The class should extends Controller base class
    ///     * If Controller part is not given, It is assumed as "Home"
    /// 2. Second part of the URL indentifes the controller Action (the job controller should do)
    ///     * Action is mapped to a method with the same name
    ///     * If Action part is not given, it is assumed as "Index"
    /// 3. Whatever Controller Action returns 
    ///     * Is known as ActionResult
    ///     * ActionResult is send to user as a View
    /// </summary>
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}