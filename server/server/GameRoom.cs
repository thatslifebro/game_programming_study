using System;
using Server;

namespace server
{
	class GameRoom
	{
		List<ClientSession> _sessions = new List<ClientSession>();

		object _lock = new object();

		public void Broadcast(ClientSession session, string message)
		{
			S_Chat packet = new S_Chat();
			packet.playerId = session.SessionId;
			packet.chat = message+$" I'm {packet.playerId}.";

			ArraySegment<byte> segment = packet.Serialize();

			lock (_lock)
			{
				foreach (ClientSession sess in _sessions)
				{
					sess.Send(segment);
				}
			}
			//recieve받자마자 broadcasting했는데 하나받고 모든사람에게 다시 전달하면 lock때문에 한번에 한쓰레드밖에 일을 못해서
			//문제가 생긴다.
		}

		public void Enter(ClientSession session)
		{
			lock (_lock)
			{
                _sessions.Add(session);
                session.Room = this;
            }
			
		}
		public void Leave(ClientSession session)
		{
            lock (_lock)
            {
                _sessions.Remove(session);
            }
            
		}

		
	}
}

