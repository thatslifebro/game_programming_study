using System;
using System.Threading;
using System.Threading.Tasks;


namespace ServerCore
{

    class Program
    {
        static int _num = 0;
        static Mutex _lock = new Mutex();

        static void Thread_1()
        {
            for(int i = 0; i < 1000000; i++)
            {
                _lock.WaitOne();
                _num++;
                _lock.ReleaseMutex();
            }
        }

        static void Thread_2()
        {
            for (int i = 0; i < 1000000; i++)
            {
                _lock.WaitOne();
                _num--;
                _lock.ReleaseMutex();
            }
        }

        static void Main(string[] args)
        {
            Task t1 = new Task(Thread_1);
            Task t2 = new Task(Thread_2);
            t1.Start();
            t2.Start();

            Task.WaitAll(t1, t2);

            Console.WriteLine(_num);
            
        }
    }
    
}
// mutex : 이것도 kernel단 까지 가는 거임. reset event 랑 비슷함
//reset event는 boolean하나 저장하지만 mutex는 lock 개수를 셀수도 잇고, thread id도 있어서
//누가 lock 걸었는지 이런 추가 정보들을 저장하고 있다.