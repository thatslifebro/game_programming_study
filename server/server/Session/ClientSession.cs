using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using static System.Collections.Specialized.BitVector32;
using ServerCore;
using server;

namespace Server
{


    class ClientSession : PacketSession
    {
        public int SessionId { get; set; }
        public GameRoom gameRoom { get; set; }
        public WaitingRoom waitingRoom { get; set; }

        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnConnected endpoint : {endPoint}");
            Program.waitingRoom.Push(() => Program.waitingRoom.Enter(this));
        }

        public override void OnRecvPacket(ArraySegment<byte> buffer)
        {
            PacketManager.Instance.OnRecvPacket(this, buffer);
        }

        public override void OnDisconnected(EndPoint endPoint)
        {
            if(Program.waitingRoom != null)
            {
                WaitingRoom room = waitingRoom;
                room.Push(() => room.Leave(this));
                waitingRoom = null;
            }
            SessionManager.Instance.Remove(this);
            Console.WriteLine($"OnDisconnected endpoint : {endPoint}");
        }




        public override void OnSend(int numOfBytes)
        {
            //Console.WriteLine($"Transferred Bytes : {numOfBytes}");
        }
    }

}