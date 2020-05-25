using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace BookWebApiServerFull
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes(); //WebApi Urls can't also be configured using Route at specific points rather a common schema

            //think of a url like--> find all authors who has written books in min-max range
            //https://books.org/api/authors/withbookcountbetween/2/and/5  <---6 part url. there may be 9part or 10 part url
            //https://covid19.org/stats/india/karnataka/bangalore/ward92 <--- 5 part url with different meaning to parts. it can be sub parts
            //we can't keep defining RouteMap for every possiblilty. so we can use attribute routes


            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
