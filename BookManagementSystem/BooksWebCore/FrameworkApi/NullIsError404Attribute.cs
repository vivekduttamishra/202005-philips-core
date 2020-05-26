using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BooksWebCore.FrameworkApi
{
    public class NullIsError404Attribute : ActionFilterAttribute
    {

        public string Reason { get; set; } = "Sorry, No Information Found";
        //My filter will be activated only after action has completed.
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var result = context.Result;
            if(result is ViewResult)
            {
                var vr = result as ViewResult; 
                if(vr.Model==null) //so ViewResult got no Model
                {
                    SetErrorContext(context);
                    //now forget what actual action suggest
                    //Take this result forward
                    context.Result = new ViewResult()
                    {
                        ViewName = "ErrorView"
                    };
                } 
            }
            else if(result is OkObjectResult)
            {
                var okr = result as OkObjectResult;
                var model = okr.Value;
                if (model == null)
                {
                    SetErrorContext(context);
                    context.Result=new NotFoundObjectResult(context.ModelState);
                }
            }

            base.OnActionExecuted(context);
        }

        private  void SetErrorContext(ActionExecutedContext context)
        {
            context.HttpContext.Response.StatusCode = 404;
            //let us change the result to something else
            context.ModelState.AddModelError("Reason", Reason);
            
            context.ModelState.AddModelError("Url", context.HttpContext.Request.Path);
            context.ModelState.AddModelError("Status", "404");

        }
    }
}
