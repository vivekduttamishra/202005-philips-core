using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelloNetCore.Code
{
    public interface IFormatterService
    {
        string Format(string message);
    }

    public class CapitalizerGreetingService : IFormatterService
    {
        IGreetService greetService;

        //Greet Service injected to this service
        public CapitalizerGreetingService(IGreetService service)
        {
            this.greetService = service;
        }

        public string Format(string name)
        {
            return greetService.Greet(name).ToUpper();
        }
    }
}
