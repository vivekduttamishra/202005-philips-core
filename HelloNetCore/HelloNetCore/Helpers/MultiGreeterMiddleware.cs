using HelloNetCore.Code;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloNetCore.Helpers
{
    public class MultiGreeterMiddleware
    {
        RequestDelegate next;
        String url="/custom";
        IEnumerable<IGreetService> services;
        public MultiGreeterMiddleware(RequestDelegate next, IEnumerable<IGreetService> services, string url)
        {
            this.next = next;
            this.services = services;
            this.url = url;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.Request.Path.StartsWithSegments(url))
            {
                await next(context);
                return;
            }

            var parts = context.Request.Path.Value.Split('/');
            string error = null;
            if (parts.Length < 2)
            {
                error = "Name Not Supplied";
                context.Response.StatusCode = 400;
            }

            var name = parts[2];

            //handle current request
            context.Response.ContentType = "text/html";

            StringBuilder html = new StringBuilder("<html><title>Welcome</title></head>");
            html.AppendFormat("<body><h1>Welcome</h1>");

            if(error!=null)
            {
                html.AppendFormat($"<p>Erros:{error}");
            }
            else
            {
                //main logic here
                //To create a list of greeting for greeting from all configured services
                html.AppendFormat("<ul>");
                foreach(var service in services)
                {
                    html.AppendFormat($"<li>{service.Greet(name)}</li>");
                }
                html.AppendFormat("</ul>");

            }

            html.AppendFormat("</body></html>");

            await context.Response.WriteAsync(html.ToString());

        }
    }

    public static class MultiGreeterMiddlewareAppExtension
    { 
        public static void UseMultigreeter(this IApplicationBuilder app, string url="/multigreet")
        {
            app.UseMiddleware<MultiGreeterMiddleware>(url);
        }
        
    }


}
