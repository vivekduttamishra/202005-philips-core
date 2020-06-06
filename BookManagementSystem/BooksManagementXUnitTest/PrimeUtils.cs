using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BooksManagementXUnitTest
{
    public class PrimeUtils
    {
        public bool IsPrime(int number)
        {
            if (number < 0)
                throw new ArithmeticException($"Number Must be Positive. Found :{number}");

            if (number < 2)
                return false;

            for (int test = 2; test < number; test++)
                if (number % 2 == 0)
                    return false;

            return true;
        }
    
        public async Task<bool> IsPrimeAsync(int number)
        {
            await Task.Delay(5);
            return IsPrime(number);
        }
    }
}
