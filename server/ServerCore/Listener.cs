﻿using System;
using System.Net;
using System.Net.Sockets;

namespace ServerCore
{
	public class Listener
	{

		Socket _listenSocket;
		Func<Session> _sessionFactory;

		public void init(Func<Session> sessionFactory, IPEndPoint endPoint)
		{
            _listenSocket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
			_sessionFactory += sessionFactory;

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
				Session session = _sessionFactory.Invoke();
                session.Start(args.AcceptSocket);
				session.OnConnected(args.AcceptSocket.RemoteEndPoint);

			}
			else
			{
				Console.WriteLine(args.SocketError.ToString());
			}
			RegisterAccept(args);
		}


	}
}

//sessionFactory로 바꿈
//엔진단과 컨텐츠단 분리 