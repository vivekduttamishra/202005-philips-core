using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelloNetCore.Helpers;
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
            if(env.IsDevelopment())
                app.UseDeveloperExceptionPage();


            app.Use(next =>
            {
                return async context =>
                {
                    try
                    {
                        await next(context);
                    }
                    catch (Exception ex)
                    {
                        context.Response.StatusCode = 500;
                        if (env.IsEnvironment("HarryPotter"))
                            await context.Response.WriteAsync("Your wand has misfired -- " + ex.Message);
                        else
                            await context.Response.WriteAsync("Muggles can't see the magic");
                    }
                };


            });

            
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


            //you have added functionality to serve the static pages like html and css

            //app.UseDefaultFiles(new DefaultFilesOptions()
            //{
            //    DefaultFileNames = new List<string>() { "home.html", "index.html" }
            //}); //configure auto search for default files like index.html or default.html
            ////now the url contains /index.html
            //app.UseStaticFiles();

            app.UseFileServer(); //internally ahndles UseDefaultFiles() and UseStaticFiles()


            app.UseMappedUrl("/time", async context =>
            {
                await Task.Delay(2500);
                var date = DateTime.Now;
                await context.Response.WriteAsync(date.ToLongTimeString());
            });

            app.UseWelcomePage("/welcome"); //It is a proof of concept. Will server the /Path

            app.UseMappedUrl("/error", context =>
            {
                throw new Exception("Something went wrong!");
            });

            

            
            app.UseMappedUrl("/date", async context =>
            {
                await Task.Delay(1500);
                await context.Response.WriteAsync(DateTime.Now.ToLongDateString());
            });

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
