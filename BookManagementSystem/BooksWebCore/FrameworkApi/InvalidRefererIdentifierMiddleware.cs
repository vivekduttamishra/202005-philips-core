using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BooksWebCore.FrameworkApi
{
    public class RefererRule
    {
        public bool EmptyRefererIsInvalid { get; set; }
        public string ValidHost { get; set; }
    }

    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class InvalidRefererIdentifier
    {
        private readonly RequestDelegate _next;
        private readonly RefererRule rule;

        public InvalidRefererIdentifier(RequestDelegate next,RefererRule rule)
        {
            _next = next;
            this.rule = rule;
        }

        public Task Invoke(HttpContext httpContext)
        {

            var referer = httpContext.Request.Headers["referer"].FirstOrDefault();

            bool emptyRefeferIsInvalid = (rule.EmptyRefererIsInvalid && string.IsNullOrEmpty(referer));
            bool invalidHost = !string.IsNullOrEmpty(referer) && !referer.Contains(rule.ValidHost);
            bool result= emptyRefeferIsInvalid || invalidHost;

            //httpContext.Items["invalid_referer"] = result;
            
            httpContext.Request.Headers["invalid_referer"]=result.ToString();
            
            return _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class InvalidRefererIdentifierExtensions
    {
        public static IApplicationBuilder UseInvalidRefererIdentifier(this IApplicationBuilder builder, Action<RefererRule> ruleModifier=null)
        {
            var rule = new RefererRule() { EmptyRefererIsInvalid = true, ValidHost = "localhost" };
            if(ruleModifier!=null)
                ruleModifier(rule); //modify the default rule

            return builder.UseMiddleware<InvalidRefererIdentifier>(rule); //to inject to the constructor of middleware
        }
    }
}
