using System;
using System.Threading;
using System.Threading.Tasks;


namespace ServerCore
{
    class SpinLock
    {
        volatile int _locked = 0;

        public void Acquire()
        {

            while (true)
            {
                int expected = 0;
                int desired = 1;
                if(Interlocked.CompareExchange(ref _locked, desired,expected)==expected)
                {
                    break;
                }
                //쉬다 올게 하면 spin lock 아닌 다른 것.
                //Thread.Sleep(1); // 무조건 1ms 휴식
                //Thread.Sleep(0); // 조건부 양보. 우선순위 높거나 같은 쓰레드에 양보. 
                Thread.Yield(); // 관대한 양보. 다 먼저 해. 실행 가능한게 아예 없으면 함.
            }
            
            
            
        }
        public void Release()
        {
            _locked = 0;
        }
    }

    class Program
    {
        static int _num = 0;
        static SpinLock _lock = new SpinLock();

        static void Thread_1()
        {
            for(int i = 0; i < 100000; i++)
            {
                _lock.Acquire();
                _num++;
                _lock.Release();
            }
        }

        static void Thread_2()
        {
            for (int i = 0; i < 100000; i++)
            {
                _lock.Acquire();
                _num--;
                _lock.Release();
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

//context switching 이란 멀티프로세스 환경에서 cpu가 interrupt요청으로 다음 프로세스를 실행할 때
//기존 프로세스의 상태나 값을 저장하고 다음 프로세스를 위한 상태나 값을 불러오는 작업을 말한다.