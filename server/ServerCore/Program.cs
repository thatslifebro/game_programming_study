using System;
using System.Threading;
using System.Threading.Tasks;


namespace ServerCore
{

    class Program
    {
        // 1. 근성  spinlock
        // 2. 양보  sleep, yield
        // 3. 갑질  auto reset event, mutex
        //     monitor class
        static object _lock = new object();
        static SpinLock _lock2 = new SpinLock();
        static Mutex _lock3 = new Mutex();
        //or 직접 만들기


        class Reward
        {

        }
        // 보상 목록 : [][][]  [][] 이렇게 있는데 여기 접근할 때 lock을 쓰면 동시다발적으로
        //받기가 어려움. 보상을 추가하거나 변경할 때, 즉 write 할 때만 lock을 걸면 좋음.
        // 개별로 거는 락, RWlock, reader writer lock
        static ReaderWriterLockSlim _lock_slim = new ReaderWriterLockSlim();
        static Reward GetRewardById(int id)
        {
            _lock_slim.EnterReadLock();
            _lock_slim.ExitReadLock();
            //writelock이 없으면 락이없는 것 처럼 동시다발적으로 읽기 가능.
            return null;
        }

        void AddReward(Reward reward)
        {
            _lock_slim.EnterWriteLock();
            _lock_slim.ExitWriteLock();
        }


        static void Main(string[] args)
        {
            lock (_lock)
            {

            }

            // spinlock 이지만 계속 해도 안되면 잠깐 쉬다옴 
            bool lockTaken = false;
            try
            {
                _lock2.Enter(ref lockTaken);
            }
            finally
            {
                if(lockTaken)
                    _lock2.Exit();
            }
        }
    }

}