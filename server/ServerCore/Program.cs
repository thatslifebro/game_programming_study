using System;
using System.Threading;
using System.Threading.Tasks;


namespace ServerCore
{
    class Program
    {
        static volatile int number = 0;

        static void Thread_1()
        {
            for(int i=0; i < 1000000; i++)
            {
                Interlocked.Increment(ref number);
                //number++;// int temp = number; temp+=1; number=temp;
            }
        }

        static void Thread_2()
        {
            for(int i = 0; i < 1000000; i++)
            {
                Interlocked.Decrement(ref number);
                //number--;// int temp = number; temp+=1; number=temp;
            }
        }

        static void Main(string[] args)
        {
            
            Task t1 = new Task(Thread_1);
            Task t2 = new Task(Thread_2);
            t1.Start();
            t2.Start();

            Task.WaitAll(t1, t2);

            Console.WriteLine(number);
        }
    }
    
}

// 결과가 0이 안나옴 . 경합조건(race condition) 쓰레드들이 동시에 같은 변수에 접근한것 ?

// 일단 ++ 할때 number 값을 temp에 저장 ++하고 다시 number에 저장한다

// 한번에 저거 3개를 해야 정상작동인데. 그러지 못해서 문제.

// 원자성 - atomic 하지 않아서 문제.

//interlocked 쓰면 원자성 만족 하지만 성능 떨어짐

//ref 는 주소값. 