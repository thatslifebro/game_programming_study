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

        static Listener _listener = new Listener();

        static void FlushRoom()
        {
            waitingRoom.Push(() => waitingRoom.Flush());
            JobTimer.Instance.Push(FlushRoom, 250);
        }

        static void Main(string[] args)
        {


            //DNS
            //string host = Dns.GetHostName();
            //IPHostEntry ipHost = Dns.GetHostEntry(host);
            //IPAddress ipAddr = ipHost.AddressList[0];
            //IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);
            IPAddress ipAddr = IPAddress.Parse("127.0.0.1");
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 8888);


            _listener.init(() => { return SessionManager.Instance.Generate(); }, endPoint);
            Console.WriteLine("Listenning...");

           JobTimer.Instance.Push(FlushRoom);
            
            while (true)
            {
                JobTimer.Instance.Flush();
                
            }

        }
    }

}
