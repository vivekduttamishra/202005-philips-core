using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HelloNetCore
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // Creates a pipeline. No code executes right now
        // But pipeline functionalites execute on the request.
        // 
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //Adding a middleware in the pipeline.

            //Middleware to Pass control to the next middleware

            app.Use(next =>
           {
               return async context =>
               {
                   var start = DateTime.Now; //dosomething before passing to next
                   await next(context);   //let nex do the job and return back
                   var end = DateTime.Now; //do something when it returns
                   Console.WriteLine("Time taken by {0} is {1}", context.Request.Path, (end - start).TotalMilliseconds);
               };
           });

            //process url /time or pass to the next middleware
            app.Use(next =>
            {
                //This is the middleware
                return async context =>
                {
                    if (context.Request.Path.StartsWithSegments("/time"))
                    {
                        await Task.Delay(2500);
                        var date = DateTime.Now;
                        await context.Response.WriteAsync(date.ToLongTimeString());
                    }
                    else
                    {
                        await next(context);
                    }
                };
            });

            //process url /date or pass to the next middleware
            app.Use(next =>
            {
                //This is the middleware
                return async context =>
                {
                    if (context.Request.Path.StartsWithSegments("/date"))
                    {
                        await Task.Delay(1500);
                        var date = DateTime.Now;
                        await context.Response.WriteAsync(date.ToLongDateString());
                    }
                    else
                    {
                        await next(context);
                    }
                };
            });


            //handle all other request <--- fallback url
            //Middleware that always returns back
            //And *NOT* passes control to the next Middleware
            //app.Run(SayHelloWorld); //will run when request comes
            
            app.Run(context =>
            {
                return context.Response.WriteAsync("Hello World from " + context.Request.Path.ToString());
            });


            app.Run(SayHelloWorld);
        }

        

        //Signaure for RequestDelegate
        public Task SayHelloWorld(HttpContext context)
        {
            //Returns an Async Respons
            return context.Response.WriteAsync("Hello World from .NET Core");
        }
    }
}
