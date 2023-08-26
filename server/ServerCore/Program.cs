using System;
using System.Threading;
using System.Threading.Tasks;


namespace ServerCore
{
    class Program
    {
        //volatile 쓰면 밑에 메모리에 바로 작업하는 것 처럼 된다
        static int x = 0;
        static int y = 0;
        static int r1 = 0;
        static int r2 = 0;

        static void Thread1()
        {
            y = 1;// Store y
            Thread.MemoryBarrier(); // 이거하면 위에거를 실제 메모리에 먼저 작업 .
            r1 = x;// load x

        }

        static void Thread2()
        {
            x = 1;// Store x
            Thread.MemoryBarrier();
            r2 = y; // load y
        }

        static void Main(string[] args)
        {
            int count = 0;
            while (true)
            {
                count++;
                x = y = r1 = r2 = 0;

                Task t1 = new Task(Thread1);
                Task t2 = new Task(Thread2);
                t1.Start();
                t2.Start();

                Task.WaitAll(t1, t2);

                if(r1==0 && r2 == 0)
                {
                    break;
                }
            }
            Console.WriteLine($"{count}번만에 빠져나옴");
        }
    }
    
}

// 쓰레드 내부에서 명령들의 순서가 달라질 수 있다. 하드웨어에 의해.
// 순서대로 하기 위해 메모리베리어 .
// Full memory barrier : asm에선 MFENCE. C#에선 MemoryBerrier() => Store, load 둘다 막음
// Store memory barrier : asm - SFENCE.
// Load memory barrier  : asm - LFEENCE.