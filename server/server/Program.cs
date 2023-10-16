using System;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Text;
using static System.Collections.Specialized.BitVector32;
using ServerCore;
using Server;
using server;

namespace Server
{
    
    class Program
    {
        public static WaitingRoom waitingRoom = new WaitingRoom();

        public static List<GameRoom> gameRooms = new List<GameRoom>();

        static Listener _listener = new Listener();

        static void FlushWaitingRoom()
        {
            waitingRoom.Push(() => waitingRoom.Flush());
            JobTimer.Instance.Push(FlushWaitingRoom, 250);
        }

        static void FlushGameRooms()
        {
            
            foreach(KeyValuePair<GameRoom,List<ClientSession>> pair in GameRoomManager.Instance._gameRooms)
            {
                pair.Key.Push(() => pair.Key.Flush());
            }
            JobTimer.Instance.Push(FlushGameRooms, 250);
        }

        static void Main(string[] args)
        {


            //DNS
            //string host = Dns.GetHostName();
            //IPHostEntry ipHost = Dns.GetHostEntry(host);
            //IPAddress ipAddr = ipHost.AddressList[0];
            IPAddress ipAddr = IPAddress.Parse("0.0.0.0");
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7676);
     


            _listener.init(() => { return SessionManager.Instance.Generate(); }, endPoint);
            Console.WriteLine("Listenning...");

            JobTimer.Instance.Push(FlushWaitingRoom);
            JobTimer.Instance.Push(FlushGameRooms);

            while (true)
            {
                JobTimer.Instance.Flush();
                
            }

        }
    }

}
