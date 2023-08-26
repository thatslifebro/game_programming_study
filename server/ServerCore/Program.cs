using System;
using System.Threading;
using System.Threading.Tasks;


namespace ServerCore
{
    class Program
    {
        static int number = 0;
        static object _obj = new object();

        static void Thread_1()
        {
            for(int i=0; i < 1000000; i++)
            {
                //Monitor.Enter(_obj);
                lock (_obj)
                {
                    number++;
                }

                // int temp = number; temp+=1; number=temp;

                //Monitor.Exit(_obj);
            }
        }

        static void Thread_2()
        {
            for(int i = 0; i < 1000000; i++)
            {
                Monitor.Enter(_obj);

                number--;// int temp = number; temp+=1; number=temp;

                Monitor.Exit(_obj);
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

// 여러 쓰레드에서 쓰기 시작하면 lock 걸어야함. 임계영역
// _obj라는 방을 만들어놓고 누군가 enter했으면 exit 하기전까진 사용 못함 (mutual exclusive)
// 관리하기가 어려움. 중간에 return 때려버리면 exit안되서 문제발생 => deadlock.
// try catch finally 로 exit 무조건 되게 할 수 있음. 하지만 좀 드럽다.
// lock을 사용할 수 있다. 