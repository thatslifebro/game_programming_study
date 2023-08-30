using System;
using System.Threading;
using System.Threading.Tasks;


namespace ServerCore
{
    class Lock
    {
        // kernel 단에서 관리하는 bool 값.
        //AutoResetEvent _available = new AutoResetEvent(true);//initial state true or false.
        ManualResetEvent _available = new ManualResetEvent(true);
        public void Acquire()
        {

            _available.WaitOne();// 입장 시도. auto reset event는 문닫기 자동.
            _available.Reset(); //이라는게 원래있음 문닫기
            // 이렇게 따로 하면 원자성이 만족안되서 return 값이 0이 안됨.
            // lock 구현 시나리오와는 맞지 않음 (manual reset event)
            
        }
        public void Release()
        {
            _available.Set(); // 문열기 .
        }
    }

    class Program
    {
        static int _num = 0;
        static Lock _lock = new Lock();

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

//제 3자가 lock이풀렷는지 확인해주고 알려주는 방법 : 제 3자가 kernel 쪽 사람이라 느림.(context switching이 필요하니)
//auto reseet event, manual reeset event
//다른 것들의 비해 느림.