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

		public void Broadcast(ArraySegment<byte> segment)
		{
			_pendingList.Add(segment);
		}

		public void Flush()
		{
			foreach (ClientSession sess in _sessions)
			{
				sess.Send(_pendingList);
			}
			//Console.WriteLine($"Flushed {_pendingList.Count} items");
			_pendingList.Clear();
		}

        public void Enter(ClientSession session)
		{
			//player추가부분 
            _sessions.Add(session);
            session.Room = this;

			//새로온애한테 모든 목록전송
			S_PlayerList players = new S_PlayerList();
			foreach(ClientSession s in _sessions)
			{
				players.players.Add(new S_PlayerList.Player()
				{
					isSelf = (s == session),
					playerId = s.SessionId,
					posX = s.PosX,
					posY = s.PosY,
					posZ = s.PosZ,
				});
			}
			session.Send(players.Serialize());

			//다른애들한테 새로온애 알려주기
			S_BroadcastEnterGame enter = new S_BroadcastEnterGame();
			enter.playerId = session.SessionId;
			enter.posX = 0;
            enter.posY = 0;
            enter.posZ = 0;
			Broadcast(enter.Serialize());


        }
		public void Leave(ClientSession session)
		{
			//player 제거 
            _sessions.Remove(session);
			// 모두에게 알린다
			S_BroadcastLeaveGame leave = new S_BroadcastLeaveGame();
			leave.playerId = session.SessionId;
			Broadcast(leave.Serialize());
		}

		public void Move(ClientSession session, C_Move movePacket)
		{
			//좌표바꾸기
			session.PosX = movePacket.posX;
            session.PosY = movePacket.posY;
            session.PosZ = movePacket.posZ;
			//모두에게 알리
			S_BroadcastMove move = new S_BroadcastMove();
			move.playerId = session.SessionId;
			move.posX = session.PosX;
            move.posY = session.PosY;
            move.posZ = session.PosZ;
			Broadcast(move.Serialize());
        }
	}
}

