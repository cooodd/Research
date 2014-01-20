using Library2010;
using System;


namespace For2010
{
    class Program
    {
        static void Main(string[] args)
        {
            TestMethodvsOperator();
            Console.Read();
        }


        /// <summary>
        /// 1千万次时间比 9：36
        /// HeapSort using method comparing VS operator comparing(1百万) 1.36 : 1
        /// </summary>
        private static void TestMethodvsOperator()
        {
            int a = int.MaxValue; int b = int.MinValue;

            for (int x = 0; x < 4; x++)
            {

                Code.Time("Operator", 1, () =>
                {
                    //bool n;
                    for (int i = 0; i < 10000000; i++)
                    {
                        bool n = a > b;
                    }
                });

                Code.Time("Method", 1, () =>
                {
                    //int n;
                    for (int i = 0; i < 10000000; i++)
                    {
                        int n = a.CompareTo(b);
                    }
                });
            }
        }
    }

}
