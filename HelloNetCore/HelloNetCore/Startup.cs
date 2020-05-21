using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelloNetCore.Code;
using HelloNetCore.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HelloNetCore
{
    public class Startup
    {

        ILogger<Object> logger;

        /*
         * We can inject dependency even in the constructor of Startup
         * However here we can inject only preconfigured dependencies as ConfigureService metrhod is not invoked yet
         * Remeber that mehtod can be invoked only after object is created and constrcutor is called.
         */ 

        //public Startup(ILogger<Object> logger)
        //{
        //    this.logger = logger;
        //}


        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940

        //CONFIGURE YOUR INJECTABLE SERVICES HERE
        //HERE YOU DEFINE ALL THE SERVICES (OBJECTS) THAT YOUR APPLICATION WILL NEED ANYWHERE
        //IN MIDDLEWARES, IN CONSTRUCTORS, IN MODELS 
        //AND CORE WILL AWAYS SUPPLY THOSE SERVICES TO WHOEVER NEEDS IT
        public void ConfigureServices(IServiceCollection services)
        {
            //Now .NET core knows that IGreetService should be mappped to HelloGreetService

            services.AddScoped<IFormatterService, CapitalizerGreetingService>(); //<--- scoped to current request


            services.AddSingleton<IGreetService, HelloGreetService>();
            services.AddSingleton<IGreetService, TimedGreetService>();
            //services.AddSingleton<IGreetService, ConfigurableGreetService>(); //<--- single object serves all request

            services.AddTransient<IGreetService, ConfigurableGreetService>();   //<--- a new object is created for every request

            //services.AddSingleton<IGreetService, ExtendedConfigurableGreetService>();
            services.AddSingleton<IGreetService, ExtendedConfigurableGreetServiceV2>();

            //logger.LogInformation("Configure Service Method completed.");
            Console.WriteLine("Configure Service Finished");

            //include MVC related interal objects such as -- ActionInvoker ModelBinder, ControllerFactory
            services.AddMvc(opt =>
            {
                opt.EnableEndpointRouting = false;
            });
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // Creates a pipeline. No code executes right now
        // But pipeline functionalites execute on the request.

        // CONFIGURE YOUR MIDDLEWARES HERE
        public void Configure(
            //Preconfigured Services
            IApplicationBuilder app, 
            IWebHostEnvironment env, 
            ILogger<Startup> logger,  //<-- already injected in the constructor of startup
            //Services configured by CongiureService method
            IGreetService greetService
            )
        {

            //Dependency Injection
            /*
             * All parameters supplied to Configure function are actually dependency injection requests
             * And .NET core framework is auto injecting these dependencies
             * The order of the call in
             *      0. Configure Builtin .NET core services like IApplicationBuilder is done by .NET core framework
             *      1. ConfigureServices   (Configure User related services like Model, Repositories etc)
             *      2. Configure (Middleware) <--- can use sercies from step 0 and step 1
             *      3. User request comes at this point
             * Few builtin services of .NET core is preconfigures
             * Configure funcation can demand any service either preconfigured or User configured
             */


            logger.LogInformation("Middleware configuration Started");
            //Adding a middleware in the pipeline.

            //Middleware to Pass control to the next middleware
            if(env.IsDevelopment())
                app.UseDeveloperExceptionPage();


            //service is auto injected to a method! here it is injected to Configure method above

            //app.UseMvcWithDefaultRoute();

            //app.UseNotFoundRecorder();
            
            app.UseMvc(routes =>
            {
                routes.MapRoute("default",
                    "{controller=home}/{action=index}/{id?}");
            });


            app.UseMultigreeter();
            
            app.UseMappedUrl("/welcome", async context =>
            {
                //we are using auto injected service called greetService
                

                var pathFragments = context.Request.Path.Value.Split('/');
                if (pathFragments.Length > 2)
                {
                    logger.LogInformation($"{greetService.GetHashCode()} serving url {context.Request.Path}");
                    var name = pathFragments[2];
                    await context.Response.WriteAsync(greetService.Greet(name));
                }
                else
                {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsync("Please specify the name for greeting as /wish/someone");
                }
            });


            app.UseMappedUrl("/wish", async context =>
            {

                //var service = new HelloGreetService();

                //approach #2 --> Get It from appbuilder
                //var service = app.ApplicationServices.GetService<IGreetService>();

                var service = app.ApplicationServices.GetService<IFormatterService>();

                var pathFragments = context.Request.Path.Value.Split('/');
                if(pathFragments.Length>2)
                {
                    var name = pathFragments[2];
                    await context.Response.WriteAsync(service.Format(name));
                }
                else
                {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsync("Please specify the name for greeting as /wish/someone");
                }
            });


            app.UseMappedUrl("/greet", async context =>
            {

                //var service = new HelloGreetService();

                //approach #1 --> Get It from context
                var service = context.RequestServices.GetService<IGreetService>();

                if (context.Request.Query.ContainsKey("name"))
                {
                    var name = context.Request.Query["name"];
                    await context.Response.WriteAsync(service.Greet(name));
                }
                else
                {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsync("Please specify the name for greeting as ?name=Someone");
                }
            });




            app.UseMappedUrl("/hello", async context =>
            {

                var service = new HelloGreetService();

                if (context.Request.Query.ContainsKey("name"))
                {
                    var name = context.Request.Query["name"];
                    await context.Response.WriteAsync(service.Greet(name));
                }
                else
                {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsync("Please specify the name for greeting as ?name=Someone");
                }
            });




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

            logger.LogInformation("Middleware configuration Finished");
        }

        

        //Signaure for RequestDelegate
        public Task SayHelloWorld(HttpContext context)
        {
            //Returns an Async Respons
            return context.Response.WriteAsync("Hello World from .NET Core");
        }
    }
}
