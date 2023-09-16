using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using ServerCore;

namespace DummyClient
{


    class ServerSession : PacketSession
    {
        public override void OnConnected(EndPoint endPoint)
        {

        }


        public override void OnDisconnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnDisconnected endpoint : {endPoint}");
        }


        public override void OnRecvPacket(ArraySegment<byte> buffer)
        {
            PacketManager.Instance.OnRecvPacket(this,buffer);
        }

        public override void OnSend(int numOfBytes)
        {
            //Console.WriteLine($"Transferred Bytes : {numOfBytes}");
        }
    }
}

//string은 크기가 가변이기 때문에 기존과 다르게 처리해야한다.
// 2byte로 string크기를 먼저 보내고 그뒤에 데이터 보내면 좋을듯.