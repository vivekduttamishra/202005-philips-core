using HelloNetCore.Code;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelloNetCore.Controllers
{
    public class GreetController : Controller
    {
        IGreetService service;

        public GreetController(IGreetService service)
        {
            this.service = service;
        }


        public string To(string id)
        {
            return service.Greet(id);
        }

        public ActionResult Page(string id)
        {
            Object message = service.Greet(id);

            return View(message);
        }
    }
}
