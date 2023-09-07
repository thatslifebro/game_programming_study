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
    class Packet
    {
        public ushort size;
        public ushort packetId;
        
    }
    class GameSession : PacketSession
    {
        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnConnected endpoint : {endPoint}");

            //Packet packet = new Packet(){ size=100,packetId=10};

            

            //ArraySegment<byte> openSegment = SendBufferHelper.Open(4096);
            //byte[] buffer = BitConverter.GetBytes(packet.size);
            //byte[] buffer2 = BitConverter.GetBytes(packet.packetId);
            //Array.Copy(buffer, 0, openSegment.Array, openSegment.Offset, buffer.Length);
            //Array.Copy(buffer2, 0, openSegment.Array, openSegment.Offset+buffer.Length, buffer2.Length);
            //ArraySegment<byte> sendBuff = SendBufferHelper.Close(buffer.Length + buffer2.Length);

            //Send(sendBuff);
            Thread.Sleep(1000);
            Disconnect();
        }

        public override void OnRecvPacket(ArraySegment<byte> buffer)
        {
            ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
            ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset+2);
            Console.WriteLine($"RecvPacketId : {id}, size : {size}");
        }

        public override void OnDisconnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnDisconnected endpoint : {endPoint}");
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
