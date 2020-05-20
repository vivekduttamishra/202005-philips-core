using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelloNetCore.Code
{
    public interface IGreetService
    {
        string Greet(string name);
    }

    public class HelloGreetService : IGreetService
    {
        public string Greet(string name)
        {
            //return string.Format("Hello {0}", name);

            return $"Hello {name}";
        }
    }


    public class TimedGreetService : IGreetService
    {
        public string Greet(string name)
        {
            var hour = DateTime.Now.Hour;
            string prefix = "Morining";
            if (hour >= 12)
                prefix = "Afternoon";
            else if (hour >= 18)
                prefix = "Evening";

            return $"Good {prefix} {name}";

        }
    }




    //reads the prefix and the suffix from configuration!
    public class ConfigurableGreetService : IGreetService
    {
       
        string prefix;
        string suffix;

        public string Greet(string name)
        {
            return $"{prefix} {name} {suffix}";
        }
    }



}
