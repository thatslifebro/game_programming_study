using System;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Text;
using static System.Collections.Specialized.BitVector32;
using ServerCore;

namespace Server
{
    class GameSession : Session
    {
        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnConnected endpoint : {endPoint}");

            byte[] sendBuff = Encoding.UTF8.GetBytes("welcome to server!");
            Send(sendBuff);
            Thread.Sleep(1000);
            Disconnect();
        }


        public override void OnDisconnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnDisconnected endpoint : {endPoint}");
        }


        public override int OnRecv(ArraySegment<byte> buffer)
        {
            string recvData = Encoding.UTF8.GetString(buffer.Array, buffer.Offset, buffer.Count);
            Console.WriteLine($"[From client] : {recvData}");
            return buffer.Count;
        }

        public override void OnSend(int numOfBytes)
        {
            Console.WriteLine($"Transferred Bytes : {numOfBytes}");
        }
    }

    class Program
    {
        static Listener _listener = new Listener();

        static void Main(string[] args)
        {
            //DNS
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);


            _listener.init(() => { return new GameSession(); }, endPoint);
            Console.WriteLine("Listenning...");

            while (true)
            {
                Thread.Sleep(1);

            }

        }
    }

}
