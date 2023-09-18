using System;
using Server;
using ServerCore;

namespace server
{
	class GameRoom : IJobQueue
	{
		List<ClientSession> _sessions = new List<ClientSession>();
		object _lock = new object();
		List<ArraySegment<byte>> _pendingList = new List<ArraySegment<byte>>();

		JobQueue _jobQueue = new JobQueue();

		public void Push(Action job)
		{
			_jobQueue.Push(job);
		}

		public void Broadcast(ClientSession session, string message)
		{
			S_Chat packet = new S_Chat();
			packet.playerId = session.SessionId;
			packet.chat = message+$" I'm {packet.playerId}.";

			ArraySegment<byte> segment = packet.Serialize();

			_pendingList.Add(segment);

			//foreach (ClientSession sess in _sessions)
			//{
			//	sess.Send(segment);
			//}
		}

		public void Flush()
		{
			foreach (ClientSession sess in _sessions)
			{
				sess.Send(_pendingList);
			}
			Console.WriteLine($"Flushed {_pendingList.Count} items");
			_pendingList.Clear();
		}

        public void Enter(ClientSession session)
		{
            _sessions.Add(session);
            session.Room = this;
		}
		public void Leave(ClientSession session)
		{
            _sessions.Remove(session);
		}
	}
}

