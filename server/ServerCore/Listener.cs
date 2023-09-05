﻿using System;
using System.Net;
using System.Net.Sockets;

namespace ServerCore
{
	class Listener
	{

		Socket _listenSocket;
		Action<Socket> _onAcceptHandler;

		public void init(Action<Socket> onAcceptHandler,IPEndPoint endPoint)
		{
            _listenSocket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _onAcceptHandler += onAcceptHandler;

            _listenSocket.Bind(endPoint);

            _listenSocket.Listen(10);

			SocketAsyncEventArgs args = new SocketAsyncEventArgs();
			args.Completed += new EventHandler<SocketAsyncEventArgs>(OnAcceptCompleted);
			
			RegisterAccept(args);
        }

		void RegisterAccept(SocketAsyncEventArgs args)
		{
			args.AcceptSocket = null;

			bool pending = _listenSocket.AcceptAsync(args);
			if (pending == false)
				OnAcceptCompleted(null, args);
			
        }

		void OnAcceptCompleted(object sender, SocketAsyncEventArgs args)
		{
			if (args.SocketError == SocketError.Success)
			{

                _onAcceptHandler.Invoke(args.AcceptSocket);


			}
			else
			{
				Console.WriteLine(args.SocketError.ToString());
			}
			RegisterAccept(args);
		}


	}
}

