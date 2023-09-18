using System;
namespace ServerCore
{
	public class SendBufferHelper
	{
		public static ThreadLocal<SendBuffer> CurrentBuffer = new ThreadLocal<SendBuffer>(()=> { return null; });

		public static int ChunckSize { get; set; } = 65535 * 100;

		public static ArraySegment<byte> Open(int reserveSize)
		{
			if (CurrentBuffer.Value == null)
				CurrentBuffer.Value = new SendBuffer(ChunckSize);
			if (CurrentBuffer.Value.FreeSize < reserveSize)
			{
				CurrentBuffer.Value = new SendBuffer(ChunckSize);
			}
			return CurrentBuffer.Value.Open(reserveSize);
		}
        public static ArraySegment<byte> Close(int usedSize)
        {
			return CurrentBuffer.Value.Close(usedSize);
        }
    }

	public class SendBuffer
	{
		//[][][][u][][][][][][]
		byte[] _buffer;
		int _usedSize = 0;

		public int FreeSize { get { return _buffer.Length - _usedSize; } }

		public SendBuffer(int chunckSize)
		{
			_buffer = new byte[chunckSize];
		}

		//reserveSize : 사용할 최대 사이즈
		public ArraySegment<byte> Open(int reserveSize)
		{
			if (reserveSize > FreeSize) return null;
			return new ArraySegment<byte>(_buffer, _usedSize, reserveSize);
		}
		//usedSize : 실제 사용한 사이즈 
		public ArraySegment<byte> Close(int usedSize)
		{
			ArraySegment<byte> segment = new ArraySegment<byte>(_buffer, _usedSize, usedSize);
			_usedSize += usedSize;
			return segment;
		}
	}
}

//쓰레드 마다 큰 공간을 할당 받고 거기에 채워 넣는다. 