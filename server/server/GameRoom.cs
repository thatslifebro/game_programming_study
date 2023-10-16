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

			S_BroadcastMove move = new S_BroadcastMove()
			{
				// 여기서 검증하고 보내야할 필요 있음. 게임이 끝났는지도 검즈
				playerId = session.SessionId,
				prevX = -1 - movePacket.prevX,
				prevY = -1 - movePacket.prevY,
				nextX = -1 - movePacket.nextX,
				nextY = -1 - movePacket.nextY,
				promotion= movePacket.promotion
			};
			Broadcast(move.Serialize());
        }
	}
}

