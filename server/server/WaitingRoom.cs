using System;
using Server;
using ServerCore;

namespace server
{
    class WaitingRoom : IJobQueue
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
            session.waitingRoom = this;

            //새로온애한테 모든 목록전송
            S_PlayerList players = new S_PlayerList();
            foreach (ClientSession s in _sessions)
            {
                bool inGame = false;
                if (s.gameRoom != null) inGame = true;
                players.players.Add(new S_PlayerList.Player()
                {
                    playerId = s.SessionId,
                    isInGame = inGame
                });
            }
            Broadcast(players.Serialize());

        }
        public void Leave(ClientSession session)
        {
            //player 제거 
            _sessions.Remove(session);
            // 모두에게 알린다
            S_PlayerList players = new S_PlayerList();
            foreach (ClientSession s in _sessions)
            {
                bool inGame = false;
                if (s.gameRoom != null) inGame = true;
                players.players.Add(new S_PlayerList.Player()
                {
                    playerId = s.SessionId,
                    isInGame = inGame
                });
            }
            Broadcast(players.Serialize());
        }

    }
}

