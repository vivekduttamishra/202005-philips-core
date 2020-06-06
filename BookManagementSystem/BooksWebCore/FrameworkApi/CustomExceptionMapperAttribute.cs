using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BooksWebCore.FrameworkApi
{
    public class CustomExceptionMapperAttribute : ExceptionFilterAttribute
    {
        public Type exceptionType;
        public int statusCode;

        public CustomExceptionMapperAttribute(Type exceptionType, int statusCode)
        {
            this.exceptionType = exceptionType;
            this.statusCode = statusCode;
        }

        //Optional Parameters
        public string ViewName { get; set; } = "ErrorView";
        //set it to false to get custom HTML Page
        //by default will send a REST response
        public bool IsApiRequest { get; set; } = true; //more popular use case

        public override void OnException(ExceptionContext context)
        {
            if (exceptionType.IsAssignableFrom(context.Exception.GetType()))
            {
                context.ExceptionHandled = true; //don't worry, I just handled it
                context.HttpContext.Response.StatusCode = statusCode;
                UpdateContext(context);
                context.ModelState.AddModelError("Message", context.Exception.Message);
                if (IsApiRequest)
                {
                    context.Result = new NotFoundObjectResult(context.ModelState);
                }
                else
                {

                    context.Result = new ViewResult()
                    {
                        ViewName = ViewName
                    };
                }
            }

        }

        public virtual void UpdateContext(ExceptionContext context)
        {
            
        }
    }
}
