using System;
using System.Threading;

namespace ServerCore
{
    //재귀적 락 허용(x->o) writelock잡은 상태에서 write락 잡는게 허용. writelock잡은 상태에서 readlock 잡기 허용. readlock잡은상태에서 writelock잡기 안됌.
    //스핀 락 (5000번 이후 -> yield)
    class Lock
	{
		const int EMPTY_FLAG = 0x00000000;
		const int WRITE_MASK = 0x7FFF0000;
		const int READ_MASK = 0x0000FFFF;
		const int MAX_SPIN_COUNT = 5000;
		// _flag = [unused(1)] [WriteThreadId(15)] writelock잡은놈  [ReadCount(16)] readlock 동시에 가진 놈들의 숫자
		int _flag = EMPTY_FLAG;
		int _writeCount = 0;

		//아무도 Writelock이나 readlock을 획득하지 않을 때, 경합해 소유권 얻기
		public void WriteLock()
		{
			//동일 쓰레드가 writelock을 획득하고 있는지 확인
			int lockThreadId = (_flag & WRITE_MASK) >> 16;
			if(lockThreadId == Thread.CurrentThread.ManagedThreadId)
			{
				_writeCount++;
				return;
			}
            int desired = (Thread.CurrentThread.ManagedThreadId << 16) & WRITE_MASK;
			while (true)
			{
				for(int i = 0; i < MAX_SPIN_COUNT; i++)
				{
					//시도해서 성공하면 리턴 
					//if (_flag == EMPTY_FLAG) // 이렇게 두개로 구분되어 있으면 안됌.
					//	_flag = desired;
					if (Interlocked.CompareExchange(ref _flag, desired, EMPTY_FLAG) == EMPTY_FLAG)
					{
						_writeCount = 1;
						return;
					}
				}
                Thread.Yield();
			}
		}

		public void WriteUnlock()
		{
			int lockCount = --_writeCount;
			if (lockCount==0)
				Interlocked.Exchange(ref _flag, EMPTY_FLAG);
		}

		//아무도 writelock을 획득하지 않으면 readcount를 1늘린다.
		public void ReadLock()
		{
            int lockThreadId = (_flag & WRITE_MASK) >> 16;
            if (lockThreadId == Thread.CurrentThread.ManagedThreadId)
			{
				Interlocked.Increment(ref _flag);
			} 
            while (true)
			{
                for (int i = 0; i < MAX_SPIN_COUNT; i++)
                {
					//if((_flag & WRITE_MASK) == 0) // 이것도 원자성만족이 안됌. 리드락 라이트랑 동시에 잡힐 수 잇음
					//{
					//	_flag += 1;
					//	return;
					//}

					//실패조건 -> write락 잡고잇으면 실행안됌, readlock 하는 놈들이 많으면 밑에 두줄 사이에 1늘려버리면 수정된 것.
					int expected = _flag & READ_MASK;
					if (Interlocked.CompareExchange(ref _flag, expected + 1, expected)==expected)
						return;
                }
                Thread.Yield();
            }
		}

		public void ReadUnlock()
		{
			Interlocked.Decrement(ref _flag);
		}
	}
}

