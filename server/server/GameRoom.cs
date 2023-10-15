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
            session.gameRoom = this;

			//새로온애한테 모든 목록전송
			if (_sessions.Count > 1)
			{
				S_ResponseMatching pkt0 = new S_ResponseMatching()
				{
					amIWhite = true,
					otherPlayerId = _sessions[1].SessionId,
				};

                S_ResponseMatching pkt1 = new S_ResponseMatching()
                {
                    amIWhite = false,
                    otherPlayerId = _sessions[0].SessionId,
                };
				_sessions[0].Send(pkt0.Serialize());
                _sessions[1].Send(pkt1.Serialize());
            }


        }
		public void Leave(ClientSession session)
		{
			//player 제거
			S_GameOver pkt0 = new S_GameOver()
			{
				Draw = false,
				youLose = true,
				youWin = false
            };
			session.Send(pkt0.Serialize());
			_sessions.Remove(session);
			Console.WriteLine("gameover packet 보내기");
            session.gameRoom = null;

            S_GameOver pkt1 = new S_GameOver()
			{
				Draw = false,
				youLose=false,
				youWin=true
			};
			_sessions[0].Send(pkt1.Serialize());
			_sessions[0].gameRoom = null;
			_sessions.Clear();
		}

		public void Move(ClientSession session, C_Move movePacket)
		{
			////좌표바꾸기
			//session.PosX = movePacket.posX;
   //         session.PosY = movePacket.posY;
   //         session.PosZ = movePacket.posZ;
			////모두에게 알리
			//S_BroadcastMove move = new S_BroadcastMove();
			//move.playerId = session.SessionId;
			//move.posX = session.PosX;
   //         move.posY = session.PosY;
   //         move.posZ = session.PosZ;
			//Broadcast(move.Serialize());
        }
	}
}

