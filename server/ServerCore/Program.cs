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
            //while (_locked==1)   이렇게 하면 0일 때 둘다 넘어가서 같이 1로 바꿀 수 있음.
            //{                    한 놈은 0->1 다른 놈은 1->1 한 경우가 있다.

            //}
            //_locked = 1;


            //while (true)  // 이거는 0을 1로 바꾼 경우만 해당. 1을 1로 바꾼 경우는 안됌.
            //{
            //    int original = Interlocked.Exchange(ref _locked, 1); // 하나의 쓰레드마다 original값이 다른것.
            //    if (original == 0)
            //        break;
            //}

            while (true)
            {
                int expected = 0;
                int desired = 1;
                if(Interlocked.CompareExchange(ref _locked, desired,expected)==expected)
                {
                    break;
                }
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

//spinlock 구현하기 