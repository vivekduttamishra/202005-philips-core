using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BooksWebCore.Hubs;
using ConceptArchitect.BookManagement;
using ConceptArchitect.BookManagement.FlatFileRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

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


            //services
            //    .AddAuthentication("CookieAuth")
            //    .AddCookie("CookieAuth", config =>
            //    {
            //        config.Cookie.Name = "GatePass"; //this will be the cookie created for authentication
            //        config.LoginPath = "/Account/Login";
            //    });

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.RequireHttpsMetadata = false;
                    opt.SaveToken = true;
                    opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                    {
                        ValidateIssuer=true,
                        ValidateAudience=true,
                        ValidAudience=Configuration["Jwt:Audience"],
                        ValidIssuer=Configuration["Jwt:Issuer"],
                        IssuerSigningKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                    };

                });
                
                


            //configure asp.net mvc (webapi) services managed by a controller
            services
                //.AddControllers() // if you need only web api and no razor view engine
                .AddControllersWithViews() //if you need both webapi and razor support
                  //Replace builtin Serializer with NewtonSoftJson
                .AddNewtonsoftJson( opt=> 
                {
                    //ingore looped references
                    opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                })
                .AddXmlDataContractSerializerFormatters()
                ;

            services.AddRazorPages();

            

            ConfigureFlatFileRepositories(services);
            //ConfigureEFRepositories(services);

            services.AddScoped<IUserAdmin, SimpleUserManager>();
            services.AddScoped<IAuthorManager, SimpleAuthorManager>();
            services.AddScoped<IBookManager, SimpleBookManager>();
            services.AddScoped<BookManagerRecordCreator>(); //no interface for this class available

            services.AddLocalization(opt =>
            {
                opt.ResourcesPath = "Resources";
            });

            services.AddSignalR();
        }

        private static void ConfigureEFRepositories(IServiceCollection services)
        {
            
        }

        private static void ConfigureFlatFileRepositories(IServiceCollection services)
        {
            services.AddSingleton<BookStore>(provider =>
            {
                var root = provider.GetService<IWebHostEnvironment>().ContentRootPath;
                var file = $"{root}/App_Data/books2.db";
                return BookStore.Load(file);
            });
            services.AddSingleton<UserStore>(provider =>
            {
                var root = provider.GetService<IWebHostEnvironment>().ContentRootPath;
                var file = $"{root}/App_Data/users.db";
                return UserStore.Load(file);
            });
            services.AddScoped<IRepository<User, string>, FlatFileUserRepository>();
            services.AddScoped<IRepository<Author, string>, FlatFileAuthorRepository>();
            services.AddScoped<IRepository<Book, String>, FlatFileBookRepository>();
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

            var supportedCultures = new List<CultureInfo>()
            {
                new CultureInfo("en"),
                new CultureInfo("hi"),
                new CultureInfo("en-US")
            };

            var localizationOptions = new RequestLocalizationOptions()
            {
                SupportedCultures = supportedCultures,
                SupportedUICultures=supportedCultures,
                DefaultRequestCulture=new RequestCulture("hi")
            };
            
            


            app.UseRequestLocalization(localizationOptions);

           

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<BookHub>("/hubs/book");
                endpoints.MapRazorPages();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
