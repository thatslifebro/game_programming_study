using System;
using Server;
using ServerCore;

namespace server
{
	class GameRoomManager
	{

		static GameRoomManager _GameRoomManager = new GameRoomManager();
		public static GameRoomManager Instance { get { return _GameRoomManager; } }


        List<ClientSession> _waitingSessions = new List<ClientSession>();
        public Dictionary<GameRoom,List<ClientSession>> _gameRooms = new Dictionary<GameRoom, List<ClientSession>>();
        object _lock = new object();

        public void PushWaitingSession(ClientSession session)
        {
            lock (_lock)
            {
                
                Console.WriteLine($"{session.SessionId} is waiting for the game");
                if (_waitingSessions.Count < 1)
                {
                    _waitingSessions.Add(session);
                }
                else if (_waitingSessions[0].SessionId == session.SessionId) return;
                else
                {
                    CreateNewGameRoom(_waitingSessions[0], session);
                    _waitingSessions.Remove(_waitingSessions[0]);
                }
            }
            

        }
        public void CreateNewGameRoom(ClientSession session1, ClientSession session2)
        {
            GameRoom newRoom = new GameRoom();
            newRoom.Push(() => newRoom.Enter(session1));
            newRoom.Push(() => newRoom.Enter(session2));

            List<ClientSession> sessions = new List<ClientSession>
            {
                session1,
                session2
            };
            _gameRooms.Add(newRoom,sessions);

            Console.WriteLine($"room created ({session1.SessionId} ,{session2.SessionId})");
        }

        public void LeaveSession(ClientSession session)
        {
            GameRoom room = session.gameRoom;
            room.Push(()=>room.Leave(session));

            _gameRooms.Remove(room);
            Console.WriteLine($"{session.SessionId} leaved the room");
        }
    }
}

