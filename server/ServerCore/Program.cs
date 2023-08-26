using System;
using System.Threading;
using System.Threading.Tasks;


namespace ServerCore
{
    class SessionManager
    {
        static object _lock = new object();

        public static void TestSession()
        {
            lock (_lock)
            {

            }
        }
        public static void Test()
        {
            lock (_lock)
            {
                UserManager.TestUser();
            }
        }
    }

    class UserManager
    {
        static object _lock = new object();

        public static void Test()
        {
            //Monitor.TryEnter() //이렇게 try catch 로 deadlock걸렷을때 빠져나올지 이건 안좋은 방법.
            lock (_lock)
            {
                SessionManager.TestSession();
            }
        }

        public static void TestUser()
        {
            lock (_lock)
            {

            }
        }
    }

    class Program
    {
        static int number = 0;
        static object _obj = new object();

        static void Thread_1()
        {
            for(int i=0; i < 10000; i++)
            {
                SessionManager.Test();
            }
        }

        static void Thread_2()
        {
            for(int i = 0; i < 10000; i++)
            {
                UserManager.Test();
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

//a b 두변수를 다른 쓰레드가 각각 lock 했음. 다른 변수를 lock하고 싶은데 안됨 . 무한 대기 . deadlock
// a 를 항상 먼저 lock걸기로 약속하면 회피 가능.
// deadlock 완전히 막을 수는 없고 발생하면 추적햇서 해결 