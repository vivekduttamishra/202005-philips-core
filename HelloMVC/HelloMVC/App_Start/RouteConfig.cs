using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace HelloMVC
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //Multiple Routes are processed by First Match Algorithm

            //http://localhost:57586/philips/daysafter/12 --> controller=philips action=daysafter id=12
            //http://localhost:57586/philips/today --> controller=philips action=today id=null
            //http://localhost:57586/philips/ --> controller=philips action=indiex id=null
            //http://localhost:57586 --> controller=home action=index id=null

            //http://localhost:57586/maths/multiply/50/20 
            //doesn't match first route (it has 4 parts)
            //falls to second route and matches --->controller=maths action=multiply x=50 y=20

            //http://localhost:57586/maths/factorial/5
            // matches route1 --> controller=maths action=factorial  x=5 (not id=5)

            //http://localhost:57586/philips/daysafter/12 
            //if first route process is 12 will become x rather than id
            //but since first route must begin with hard coded word 'solve' its a mismatch
            //falls back to second (DefaultRoute)

            routes.MapRoute(
                name: "MathRoute",
                url: "solve/{x}/{action}/{y}",
                defaults: new
                {
                    controller = "Maths",
                    action = "Index",
                    x = 0,
                    y = 0
                }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { 
                    controller = "Philips", 
                    action = "Index", 
                    id = UrlParameter.Optional 
                }
            );

            
        }
    }
}
