using System;
using System.Threading;
using System.Threading.Tasks;


namespace ServerCore
{

    class Program
    {
        //static string ThreadName; // 모든 쓰레드가 공유
        static ThreadLocal<string> ThreadName = new ThreadLocal<string>(() => { return $"{Thread.CurrentThread.ManagedThreadId}"; });//ThreadName.Value가 null일 때 이부분이 실행되는 것

        static void WhoAmI()
        {
            //ThreadName.Value = $"My name is {Thread.CurrentThread.ManagedThreadId}";
            bool repeat = ThreadName.IsValueCreated;
            if (repeat)
            {
                Console.WriteLine(ThreadName.Value+"repeat");
            } else
            {
                Console.WriteLine(ThreadName.Value);
            }
            

            
        }

        static void Main(string[] args)
        {
            ThreadPool.SetMinThreads(1, 1);
            ThreadPool.SetMaxThreads(3, 3);
            Parallel.Invoke(WhoAmI, WhoAmI, WhoAmI, WhoAmI, WhoAmI, WhoAmI);


            ThreadName.Dispose();
        }
    }

}

//tls