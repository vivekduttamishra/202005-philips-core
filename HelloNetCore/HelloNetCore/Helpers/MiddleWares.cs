using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelloNetCore.Helpers
{
    
    public static class MiddleWares
    {
        public delegate Task FilterDelegate(RequestDelegate next);

        public static void UseBefore(this IApplicationBuilder app, RequestDelegate handler)
        {
            app.Use( next =>
            {
                return async context =>
                {
                    await handler(context);
                    await next(context);
                };
            });
        }

        public static void UseAfter(this IApplicationBuilder app, RequestDelegate handler)
        {
            app.Use(next =>
            {
                return async context =>
                {   
                    await next(context);
                    await handler(context);
                };
            });
        }

        public static void UseAround(this IApplicationBuilder app, FilterDelegate handler)
        {
            
            app.Use(next =>
            {
                return async context =>
                {
                    await handler(next);
                };
            });
        }

        public static void UseMappedUrl(this IApplicationBuilder app,string url, RequestDelegate handler)
        {
            //This is the middleware
            app.Use(next =>
            {
                //This is the middleware
                return async context =>
                {
                    if (context.Request.Path.StartsWithSegments(url))
                    {
                        await handler(context);
                    }
                    else
                    {
                        await next(context);
                    }
                };
            });

        }

    }
}
