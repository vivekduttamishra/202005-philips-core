using System;
using System.Web.Http;
using System.Web.Mvc;

namespace BooksWebFull
{
    internal class WebApiConfig
    {
        internal static void Configure(HttpConfiguration configuration)
        {
            configuration.Routes.MapHttpRoute("ApiRoute",
                "api/{controller}/{id}",
                new { controller = "Home", Id = UrlParameter.Optional }
                );
        }

        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}