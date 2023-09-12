using System;
using ServerCore;

namespace Server
{
	class PacketHandler
	{
		public static void PlayerInfoReqHandler(PacketSession session, IPacket packet)
		{
			PlayerInfoReq p = packet as PlayerInfoReq;
            Console.WriteLine($"PlayerInfoReq playerId : {p.playerId}");
            Console.WriteLine($"PlayerInfoReeq name : {p.name}");

            foreach (PlayerInfoReq.Skill skill in p.skills)
            {
                Console.WriteLine($"skill : {skill.id}, {skill.level}, {skill.duration}");
                foreach (PlayerInfoReq.Skill.Attr att in skill.attrs)
                {
                    Console.WriteLine($"skill : {skill.id}, {skill.level}, {skill.duration},{att.attrNum}");
                }
            }
        }
	}
}

