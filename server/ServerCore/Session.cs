using System;
using System.Net.Sockets;
using System.Text;

namespace ServerCore
{
	class Session
	{
		Socket _socket;
		int _disconnected = 0;
        List<ArraySegment<byte>> _pendingList = new List<ArraySegment<byte>>();
        SocketAsyncEventArgs _sendArgs = new SocketAsyncEventArgs();
        SocketAsyncEventArgs _recvArgs = new SocketAsyncEventArgs();
        Queue<byte[]> _sendQueue = new Queue<byte[]>();
		

		object _lock = new object();

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

						Console.WriteLine($"Transferred Bytes : { _sendArgs.BytesTransferred}");
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
					string recvData = Encoding.UTF8.GetString(args.Buffer, args.Offset, args.BytesTransferred);
					Console.WriteLine($"[From client] : {recvData}");
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

// send 모아 보내기. 하지만 packet 모아보내기는 또 다름. 한 유저의 정보를 모두가 알아야 함. 이런것 들을 뭉쳐야 함.
// 아주 짧은 시간동안 모두의 정보를 모은 packet들을 모아 보내는 것이 좋다.
// 서버에서 할건지 콘텐츠 단에서 모아서 send한번 할건지 갈린다고 한다.
// 엔진은 여기서 끝내고, zone에 있는 모든 것들을 모아 보내는 것이 좋다고 강사님은 생각하심.