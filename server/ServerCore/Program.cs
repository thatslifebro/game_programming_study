using System;
using System.Threading;
using System.Threading.Tasks;

namespace ServerCore 
{
    class Program
    {
        static void Main(string[] args)
        {
            int[,] arr = new int[10000, 10000];
            {
                long now = DateTime.Now.Ticks;
                for (int i=0; i<10000; i++)
                {
                    for(int j = 0; j < 10000; j++)
                    {
                        arr[i, j] = 1;
                    }
                }
                long end = DateTime.Now.Ticks;
                Console.WriteLine($"(x,y) 걸린시간 {end - now}");
            }

            {
                long now = DateTime.Now.Ticks;
                for (int i = 0; i < 10000; i++)
                {
                    for (int j = 0; j < 10000; j++)
                    {
                        arr[j, i] = 0;
                    }
                }
                long end = DateTime.Now.Ticks;
                Console.WriteLine($"(y,x) 걸린시간 {end - now}");
            }
        }
    }
} 