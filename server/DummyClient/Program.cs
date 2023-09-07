using System;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Text;
using ServerCore;

namespace DummyClient
{
    class Packet
    {
        public ushort size;
        public ushort packetId;

    }
    class GameSession : Session
    {
        public override void OnConnected(EndPoint endPoint)
        {
            Packet packet = new Packet() { size = 4, packetId = 10 };
            //send
            for (int i = 0; i < 5; i++)
            {
                ArraySegment<byte> openSegment = SendBufferHelper.Open(4096);
                byte[] buffer = BitConverter.GetBytes(packet.size);
                byte[] buffer2 = BitConverter.GetBytes(packet.packetId);
                Array.Copy(buffer, 0, openSegment.Array, openSegment.Offset, buffer.Length);
                Array.Copy(buffer2, 0, openSegment.Array, openSegment.Offset + buffer.Length, buffer2.Length);
                ArraySegment<byte> sendBuff = SendBufferHelper.Close(packet.size);

                Send(sendBuff);
            }
           
        }


        public override void OnDisconnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnDisconnected endpoint : {endPoint}");
        }


        public override int OnRecv(ArraySegment<byte> buffer)
        {
            string recvData = Encoding.UTF8.GetString(buffer.Array, buffer.Offset, buffer.Count);
            Console.WriteLine($"[From Server] : {recvData}");
            return buffer.Count;
        }

        public override void OnSend(int numOfBytes)
        {
            Console.WriteLine($"Transferred Bytes : {numOfBytes}");
        }
    }

    class Program
    {

        static void Main(string[] args)
        {
            //DNS
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

            Thread.Sleep(1000);

            Connector connector = new Connector();

            connector.Connect(endPoint, () => { return new GameSession(); });

            
            while (true)
            {
                try
                {
                    Thread.Sleep(1);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }

                Thread.Sleep(100);
            }
            

            

        }

    }
}
//socket programming ex