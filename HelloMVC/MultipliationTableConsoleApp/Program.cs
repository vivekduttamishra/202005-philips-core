using ConceptArchitect.Calculations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultipliationTableConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var kb = new Input();
            var number = kb.Read<int>("Number?");
            var max = kb.Read<int>("max[default 10]?", 10);

            var table = MultiplicationTable.Generate(number, max);

            foreach(var result in table.Results)
                Console.WriteLine("{0} x {1} = {2}",result.Left,result.Right,result.Result);


        }
    }
}
