using System;
using System.Linq;


namespace For2010
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] array = new int[] { 99, 55, 66, 33, 9, -44, 77, 888, 222, 10 };

            ShellSort(array);
Console.WriteLine("你怎么这么强");
            Console.WriteLine(String.Join(",",array.ToList()));
            Console.Read();

        }

        static void ShellSort(int[] array)
        {
            int step = 0;
            while (step <= array.Length / 3)
            {
                step = step * 3 + 1;
            }

            while (step > 0)
            {
                for (int i = 0; i < step; i++)
                {
                    for (int index = i; index < array.Length; index += step)
                    {
                        int next = index + step;
                        if (next >= array.Length) break;

                        int nextValue = array[next];

                        for (int sortedIndex = i; sortedIndex < next; sortedIndex += step)
                        {
                            if (nextValue < array[sortedIndex])
                            {
                                for (int n = next; n > sortedIndex; n -= step)
                                {
                                    array[n] = array[n - step];
                                }
                                array[sortedIndex] = nextValue;
                                break;
                            }
                        }
                    }
                }
                step = (step - 1) / 3;
            }
        }

        static void Swap<T>(T[] array, int n1, int n2)
        {
            var temp = array[n1];
            array[n1] = array[n2];
            array[n2] = temp;
        }
    }

}
