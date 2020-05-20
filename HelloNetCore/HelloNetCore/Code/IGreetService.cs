using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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
        IConfiguration config;
        ILogger<ConfigurableGreetService> logger;

        //constructor called by service injector
        //it can inject other available services here!
       public ConfigurableGreetService(IConfiguration config, ILogger<ConfigurableGreetService> logger)
        {
            this.config = config;
            this.logger = logger;
            this.logger.LogInformation($"Configurable Greet Service {GetHashCode()} created");
        }


        public string Greet(string name)
        {
            prefix = config["prefix"];
            suffix = config["suffix"];
            this.logger.LogInformation($"Configurable Greet Service {GetHashCode()} serving request for name {name}");
            return $"{prefix} {name} {suffix}";
        }
    }


  


    public class ExtendedConfigurableGreetService : IGreetService
    {
        string prefix;
        string suffix;
        bool timedGreet;
        public string Prefix
        {
            get
            {
                if (!timedGreet)
                    return prefix;

                var hour = DateTime.Now.Hour;
                if (hour < 12)
                    return "Good Morning";
                if (hour >= 12)
                    return  "Good Afternoon";
                else 
                    return "Good Evening";
            }
        }


        public ExtendedConfigurableGreetService(IConfiguration config)
        {
            this.prefix = config["greetings:Prefix"]; //next config key
            this.suffix = config["greetings:Suffix"];
            this.timedGreet = bool.Parse(config["greetings:TimedGreet"]);

        }

        public string Greet(string name) 
        {
            return $"{Prefix} {name} {suffix}";
        }
    }


    public class GreetConfig
    {
        public string Suffix { get; set; }
        public bool TimedGreet { get; set; }

        private string prefix;

        public string Prefix
        {
            get {
                if (!TimedGreet)
                    return prefix;

                var hour = DateTime.Now.Hour;
                if (hour < 12)
                    return "Good Morning";
                if (hour >= 12)
                    return "Good Afternoon";
                else
                    return "Good Evening";
            }
            set { prefix = value; }
        }

    }


    public class ExtendedConfigurableGreetServiceV2 : IGreetService
    {
        GreetConfig greetConfig;
        
        public ExtendedConfigurableGreetServiceV2(IConfiguration config)
        {
            greetConfig = new GreetConfig();

            config.Bind("greetings", greetConfig);

        }

        public string Greet(string name)
        {
            return $"{greetConfig.Prefix} {name} {greetConfig.Suffix}";
        }
    }





}
