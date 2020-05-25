using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConceptArchitect.BookManagement
{
    public class IdTool
    {
       

     public static string Normalize(string p)
        {
            //Jeffrey Archer --> jeffrey-archer
            return p.ToLower().Replace(" ", "-");
        }
    }
}
