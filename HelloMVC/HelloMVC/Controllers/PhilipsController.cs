using HelloMVC.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace HelloMVC.Controllers
{
    /// <summary>
    /// How is the url http://localhost:57586/Philips/Hello Processed    /// 
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
    public class PhilipsController : Controller
    {
       


        public void Hello()
        {
            //Console.WriteLine("Hello World from Philips Controller");
        }

        public string Greet()
        {
            return "Hello World, Welcome to ASP.NET MVC!";
        }

        public DateTime Now()
        {
            return DateTime.Now; 
        }

        public Person Contact()
        {
            var person = new Person()
            {
                Name = "Vivek Dutta Mishra",
                Email = "vivek@conceptarchitect.in"
            };
            return person;
        }

        public ContentResult AdminContact()
        {
            var person = new Person()
            {
                Name = "Admin",
                Email = "admin@philips.com"
            };
            //return person;
            return new ContentResult()
            {
                Content = person.ToString(),
                ContentType="text/plain"   //data is a plain text (Not HTML)
            };

        }
    
        public ContentResult Welcome()
        {
            //1. Get Model Data
            var title = "Welcome to Philips Server";
            var date = DateTime.Now;

            //2. Generate the View
            var html = new StringBuilder();
            html.AppendFormat("<html><head><title>{0}</title></head>", title);
            html.AppendFormat("<body><h1>{0}</h1>", title);
            html.AppendFormat("<p>Date is <strong>{0}</strong></p>", date.ToLongDateString());
            html.AppendFormat("<p>Time is <strong>{0}</strong></p>", date.ToLongTimeString());
            html.AppendFormat("</body></html>");

            /*return new ContentResult()
            {
                Content = html.ToString()
            };*/
            //3. Return the View
            return Content(html.ToString()); //Content is a helper function in Controller base class

        }

        public ViewResult Home()
        {
            return View();//conventionally View name is same as action name if not specified
        }
        public ViewResult Index()
        {
            //A view should have one model not many
            //ViewData and ViewBag can have multiple data and that makes them unsuitable for 
            //passing model info.
            ViewData["date"] = DateTime.Now;
            ViewData["title"] = "Phillips Date Server";  //ViewData is original dictionary
            ViewBag.SupportEmail = "support@philips.com"; //ViewBag is introduced in asp.net mvc 2 with dynamic types
            ViewBag.SupportEmailLabel = "Contact Support";
            return View("DateTimeServer"); //can speicify view name different from action name
        }
      
        public ViewResult Today()
        {
            //step 1 - controller gets Model Data
            var date = DateTime.Now; //model

            //step 2. controller passes Model Data and ViewTemplate 
            //to "View" Engine.

            return View("DateTimeView",  //View Template
                        date             //Model Data
                        );
        }

        public ViewResult Tomorrow()
        {
            var date = DateTime.Now.AddDays(1);
            return View("DateTimeView", date);
        }

        public ViewResult AfterDays(int days)
        {
            var date = DateTime.Now.AddDays(days);
            return View("DateTimeView", date);
        }

        public string Age(DateTime birthDate)
        {
            var age = DateTime.Now - birthDate;

            return string.Format("Your age  is {0} Years", (int)(age.Days / 365));
        }


        //parameter becomes null if no value is passed or non-int value is passed.
        //but ordinary 'int' is not-nullable.
        //let us us nullable int.
        public ActionResult DaysAfter(int? id)
        {
            if(id!=null) //Happy Path!
            {
                var date = DateTime.Now.AddDays((int)id); 
                return View("DateTimeView", date);
            }

            //What if no value or invalid valud is supplied for days?

            //Approach 1 -Send plain error message to user
            //return Content("Invalid/Missing Parameter. Correct usage ?days=12");

            //Approach 2 -Send a formatted html error message to user
            //Response.StatusCode = 400; //send the right status code
            //return View(
            //    "ErrorView",   //ViewName
            //    (object) "Invalid/Missing Parameter 'days' try using ?days=7" //Model
            //    );

            //Approach 3 - we may display default/todays date
            //return Today();  //Internal transfer to a different content generation


            //Appraoch 4 -- send user to Today url using redirect

            return RedirectToAction("Today"); //Send user to Today action

            
        }

    }
}