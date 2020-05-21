using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConceptArchitect.BookManagement;
using ConceptArchitect.BookManagement.FlatFileRepository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BooksWebCore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            //var store = BookStore.Load(@"c:\temp\books.db");
            //var rep = new FlatFileAuthorRepository(store);
            //authorManager = new SimpleAuthorManager(rep);


            //To create object using special mechanism and not simple constructor call
            services.AddSingleton<BookStore>(provider =>
            {
                var root= provider.GetService<IWebHostEnvironment>().ContentRootPath;
                var file = $"{root}/App_Data/books.db";
                return BookStore.Load(file);
            });

            services.AddScoped<IRepository<Author, string>, FlatFileAuthorRepository>();
            services.AddScoped<IAuthorManager, SimpleAuthorManager>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();
            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
