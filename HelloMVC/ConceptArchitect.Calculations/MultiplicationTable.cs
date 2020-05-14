using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;


namespace ConceptArchitect.Calculations
{
    public class MultiplicationTable
    {
        public int Number { get; set; }
        public int Max { get; set; }

        public List<BinaryOperationInfo> Results { get; set; } = new List<BinaryOperationInfo>();

        public static MultiplicationTable Generate(int number,int max=10)
        {
            var table = new MultiplicationTable()
            {
                Number = number,
                Max = max
            };
            for (int i = 1; i <= max; i++)
                table.Results.Add(new BinaryOperationInfo()
                {
                    Left = number,
                    Right = i,
                    Result=number*i
                });

            return table;
        }
    }

    public class BinaryOperationInfo
    {
        public int Left { get; set; }
        public int Right { get; set; }
        public int Result { get; set; }

    }

}