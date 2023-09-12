using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using ServerCore;

namespace DummyClient
{


    class ServerSession : Session
    {
        public override void OnConnected(EndPoint endPoint)
        {
            PlayerInfoReq packet = new PlayerInfoReq() { playerId = 1001, name = "김성연"};
            var skill = new PlayerInfoReq.Skill() { id = 1, level = 10, duration = 27.3f };
            skill.attrs.Add(new PlayerInfoReq.Skill.Attr() { attrNum = 99 });
            packet.skills.Add(skill);

            packet.skills.Add(new PlayerInfoReq.Skill() { id = 2, level = 7, duration = 13.2f });
            packet.skills.Add(new PlayerInfoReq.Skill() { id = 3, level = 8, duration = 17.1f });
            packet.skills.Add(new PlayerInfoReq.Skill() { id = 4, level = 13, duration = 22.1f });

            ArraySegment<byte> s = packet.Serialize();
            if (s != null)
                Send(s);
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
}

//string은 크기가 가변이기 때문에 기존과 다르게 처리해야한다.
// 2byte로 string크기를 먼저 보내고 그뒤에 데이터 보내면 좋을듯.