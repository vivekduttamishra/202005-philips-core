using System;
using System.Collections.Generic;
using System.Text;

namespace BooksManagementXUnitTest
{
    public class PrimeData
    {
        public int Value { get; set; }
        public bool ExpectedResult { get; set; }

    }
    public class PrimeTestCaseData
    {
        public static List<Object[]> PrimeDataList { get; set; }

        static PrimeTestCaseData()
        {
            PrimeDataList = new List<Object[]>()
            {
                new Object[]{new PrimeData(){Value=2, ExpectedResult=true } },
                new Object[]{new PrimeData(){Value=1, ExpectedResult=true } }, //intentionally wrong. change is later
                new Object[]{new PrimeData(){Value=0, ExpectedResult=false } },
                new Object[]{new PrimeData(){Value=12, ExpectedResult = false } },
                new Object[]{new PrimeData(){Value=3, ExpectedResult = true } }

            };
        }


    }
}
