using System;
namespace ServerCore
{
	public class RecvBuffer
	{
		//[r][][][][][w][][][][]  w에서 이어서 클라의 메시지 받기. r은 컨텐츠단이 처리를 할 때 이동.
		ArraySegment<byte> _buffer;
		int _readPos;
		int _writePos;

		public RecvBuffer(int bufferSize)
		{
			_buffer = new ArraySegment<byte>(new byte[bufferSize],0,bufferSize);
		}
		public int DataSize { get { return _writePos-_readPos; } }
		public int FreeSize { get { return _buffer.Count - _writePos; } }

		public ArraySegment<byte> ReadSegment {
			get { return new ArraySegment<byte>(_buffer.Array,_buffer.Offset+_readPos,DataSize); }
		}
        public ArraySegment<byte> WriteSegment {
			get { return new ArraySegment<byte>(_buffer.Array,_buffer.Offset+_writePos,FreeSize); }
		}
		public void Clean()
		{
			int dataSize = DataSize;
			if (dataSize == 0)// read할 데이터가 없다면 
			{
				_readPos = 0;
				_writePos = 0;
			}
			else // read할 데이터가 남아 있으면 맨앞으로 복사해 와야 한다.
			{
				Array.Copy(_buffer.Array, _buffer.Offset + _readPos, _buffer.Array, _buffer.Offset, dataSize);
				_readPos = 0;
				_writePos = dataSize;
			}
		}

		public bool OnRead(int numOfBytes)
		{
			if (numOfBytes > DataSize) return false;
			_readPos += numOfBytes;
			return true;
		}

        public bool OnWrite(int numOfBytes)
        {
            if (numOfBytes > FreeSize) return false;
            _writePos += numOfBytes;
			return true;
        }
    }
}

//데이터를 받을 때 buffer에 저장해 놓고, 의미가 잇을 만큼 충분한 데이터가 모이면
//컨텐츠 단에서 사용할 것이고 아니라면 놔둔다.
//buffer에 어디 까지 데이터를 모앗는지 _writePos, 어디까지 데이터를 사용햇는지 _readPos 로 표시.