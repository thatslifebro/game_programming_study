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
        public static GameRoom Room = new GameRoom();

        static Listener _listener = new Listener();

        static void Main(string[] args)
        {
            

            //DNS
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);


            _listener.init(() => { return SessionManager.Instance.Generate(); }, endPoint);
            Console.WriteLine("Listenning...");

            while (true)
            {
                Thread.Sleep(1);

            }

        }
    }

}
