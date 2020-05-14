using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultipliationTableConsoleApp
{
    public class Input
    {
        public T Read<T>(String prompt, T defaultValue = default(T))
        {
            try
            {
                Console.Write(prompt);
                return (T)Convert.ChangeType(Console.ReadLine(), typeof(T));
            }
            catch
            {
                return defaultValue;
            }
        }
    }
}
