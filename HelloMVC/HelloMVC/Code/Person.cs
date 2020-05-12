using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelloMVC.Code
{
    public class Person
    {
        public string Name { get; set; }
        public string Email { get; set; }

        public override string ToString()
        {
            //Vivek Dutta Mishra<vivek@conceptarchitect.in>
            return string.Format("{0}<{1}>", Name, Email);
        }
    }
}