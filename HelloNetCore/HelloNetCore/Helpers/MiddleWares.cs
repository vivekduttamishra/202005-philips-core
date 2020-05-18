using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelloNetCore.Helpers
{
    public static class MiddleWares
    {
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
