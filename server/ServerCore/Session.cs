using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ServerCore
{
    public abstract class Session
	{
		Socket _socket;
		int _disconnected = 0;

		RecvBuffer _recvBuffer = new RecvBuffer(1024);

        List<ArraySegment<byte>> _pendingList = new List<ArraySegment<byte>>();
        SocketAsyncEventArgs _sendArgs = new SocketAsyncEventArgs();
        SocketAsyncEventArgs _recvArgs = new SocketAsyncEventArgs();
        Queue<byte[]> _sendQueue = new Queue<byte[]>();
		

		object _lock = new object();

		public abstract void OnConnected(EndPoint endPoint);
		public abstract int OnRecv(ArraySegment<byte> buffer);
		public abstract void OnSend(int numOfBytes);
		public abstract void OnDisconnected(EndPoint endPoint);


        public void Start(Socket socket)
		{
			_socket = socket;

			_recvArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnRecvCompleted);

            _sendArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnSendCompleted);

            RegisterRecv();
		}

		public void Send(byte[] sendBuffer)
		{
			//_socket.Send(sendBuffer);
			lock (_lock)
			{
                _sendQueue.Enqueue(sendBuffer);
                if (_pendingList.Count() == 0)
                    RegisterSend();
            }
        }

		public void Disconnect()
		{
			if(Interlocked.Exchange(ref _disconnected, 1) == 1)
			{
				return;
			}
			OnDisconnected(_socket.RemoteEndPoint);
			_socket.Shutdown(SocketShutdown.Both);
			_socket.Close();
		}

        #region network communication

		void RegisterSend()
		{
            while (_sendQueue.Count > 0)
			{
                byte[] buff = _sendQueue.Dequeue();
                _pendingList.Add(new ArraySegment<byte>(buff, 0, buff.Length));
			}
			_sendArgs.BufferList = _pendingList;


            bool pending = _socket.SendAsync(_sendArgs);
			if (pending == false)
				OnSendCompleted(null, _sendArgs);
		}

		void OnSendCompleted(object sender,SocketAsyncEventArgs args)
		{
			lock (_lock)
			{
                if (args.BytesTransferred > 0 && args.SocketError == SocketError.Success)
                {
                    try
                    {
						_sendArgs.BufferList = null;
						_pendingList.Clear();
						if (_sendQueue.Count > 0)
							RegisterSend();

						OnSend(_sendArgs.BytesTransferred);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"OnSendCompleted failed : {e}");
                    }
                }
                else
                {
                    Disconnect();
                }
            }
			
		}


        void RegisterRecv()
		{
			_recvBuffer.Clean();
			ArraySegment<byte> segment = _recvBuffer.WriteSegment;
			_recvArgs.SetBuffer(segment.Array, segment.Offset, segment.Count);

			bool pending = _socket.ReceiveAsync(_recvArgs);
			if (pending == false)
				OnRecvCompleted(null, _recvArgs);
		}

		void OnRecvCompleted(object sender,SocketAsyncEventArgs args)
		{
			if (args.BytesTransferred > 0 && args.SocketError == SocketError.Success)
			{
				try
				{
					//writePos 이동
					if (_recvBuffer.OnWrite(args.BytesTransferred) == false)
					{
						Disconnect();
						return;
					}

					//컨텐츠쪽으로 데이터 넘겨주고 얼마나 받았는지 알아야 함.

                    int processLen = OnRecv(_recvBuffer.ReadSegment);
					if (processLen < 0 || processLen>_recvBuffer.DataSize)
					{
						Disconnect();
						return;
					}

					//_readPos 이동
					if (_recvBuffer.OnRead(processLen) == false)
					{
						Disconnect();
						return;
					}

                    RegisterRecv();
				}catch(Exception e)
				{
					Console.WriteLine($"OnRecvCompleted failed : {e}");
				}
            }
			else
			{
				Disconnect();
			}
			
		}

        #endregion
    }
}

//데이터를 받았을 때 buffer의 어디에 저장할지 정해주고 전송이 끝나면 그 데이터가 컨텐츠쪽에서 얼마나
//사용햇는지 확인해야 한다.