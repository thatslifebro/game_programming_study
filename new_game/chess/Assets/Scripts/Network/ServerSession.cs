using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using ServerCore;
using UnityEngine;

namespace DummyClient
{


    class ServerSession : PacketSession
    {
        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnConnected endpoint : {endPoint}");
            Debug.Log($"OnConnected endpoint : {endPoint}");
        }


        public override void OnDisconnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnDisconnected endpoint : {endPoint}");
        }


        public override void OnRecvPacket(ArraySegment<byte> buffer)
        {
            PacketManager.Instance.OnRecvPacket(this,buffer, (s, p) => PacketQueue.Instance.Push(p));
        }

        public override void OnSend(int numOfBytes)
        {
            //Console.WriteLine($"Transferred Bytes : {numOfBytes}");
        }
    }
}