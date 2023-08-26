using System;
using System.Threading;
using System.Threading.Tasks;


namespace ServerCore
{
    class Program
    {
        int _answer;
        bool _complete;

        void A()
        {
            _answer = 123;
            Thread.MemoryBarrier(); //1 store 
            _complete = true;
            Thread.MemoryBarrier(); //2 store
        }

        void B()
        {
            Thread.MemoryBarrier(); //3 load 전 
            if (_complete)
            {
                Thread.MemoryBarrier();//4 load 전
                Console.WriteLine(_answer);
            }
        }

        static void Main(string[] args)
        {
            
        }
    }
    
}
// barrier 가 4개 모두 있어야 정상작동.
// store하고 난 후 , load 전.