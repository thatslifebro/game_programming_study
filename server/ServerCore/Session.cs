using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ServerCore
{
    abstract class Session
	{
		Socket _socket;
		int _disconnected = 0;
        List<ArraySegment<byte>> _pendingList = new List<ArraySegment<byte>>();
        SocketAsyncEventArgs _sendArgs = new SocketAsyncEventArgs();
        SocketAsyncEventArgs _recvArgs = new SocketAsyncEventArgs();
        Queue<byte[]> _sendQueue = new Queue<byte[]>();
		

		object _lock = new object();

		public abstract void OnConnected(EndPoint endPoint);
		public abstract void OnRecv(ArraySegment<byte> buffer);
		public abstract void OnSend(int numOfBytes);
		public abstract void OnDisconnected(EndPoint endPoint);


        public void Start(Socket socket)
		{
			_socket = socket;

            _recvArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnRecvCompleted);
            _recvArgs.SetBuffer(new byte[1024], 0, 1024);

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
                    OnRecv(new ArraySegment<byte>(args.Buffer, 0, args.Buffer.Length));
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

// engine과 content를 분리 하기 onconnected, onDisconnected, OnRecv, Onsend 로
// 4가지 상황에 무엇을 할지를 분리했고 servercore파일에 GameSession에 정의하도록 해놨다.