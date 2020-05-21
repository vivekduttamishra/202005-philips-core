using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BooksWebCore.Controllers
{
    public class HelpController 
    {
        public string Index()
        {
            return "<h1>Welcome To Book's Web Help Page</h1>";
        }

        public ContentResult Home()
        {
            return new ContentResult() 
            { 
                Content = "<h1>Welcome To Book's Web Help Page</h1>" ,
                ContentType="text/html", //text/plain is the default in .NET Core

            };
        }
        
    }
}
