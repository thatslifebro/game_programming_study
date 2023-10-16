using System;
namespace DummyClient
{
	class SessionManager
	{
		static SessionManager _session = new SessionManager();
		public static SessionManager Instance { get { return _session; } }

		List<ServerSession> _sessions = new List<ServerSession>();

		object _lock = new object();

		Random _rand = new Random();

		public void SendForEach()
		{
			lock (_lock)
			{
				foreach(ServerSession session in _sessions)
				{
					C_RequestMatching movePacket = new C_RequestMatching();
					session.Send(movePacket.Serialize());
				}

				Thread.Sleep(5000);

                foreach (ServerSession session in _sessions)
                {
                    C_Move movePacket = new C_Move()
                    {
                        prevX = -3,
                        prevY = -3,
                        nextX = -3,
                        nextY = -2,
                        promotion = -1
                    };

                    session.Send(movePacket.Serialize());
                }
            }
		}

		public ServerSession Generate()
		{
			lock (_lock)
			{
				ServerSession session = new ServerSession();
				_sessions.Add(session);
				return session;
			}
		}
	}
}

