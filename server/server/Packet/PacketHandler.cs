using System;
using ServerCore;


class PacketHandler
{
	public static void C_PlayerInfoReqHandler(PacketSession session, IPacket packet)
	{
		C_PlayerInfoReq p = packet as C_PlayerInfoReq;
        Console.WriteLine($"PlayerInfoReq playerId : {p.playerId}");
        Console.WriteLine($"PlayerInfoReeq name : {p.name}");

        foreach (C_PlayerInfoReq.Skill skill in p.skills)
        {
            Console.WriteLine($"skill : {skill.id}, {skill.level}, {skill.duration}");
            foreach (C_PlayerInfoReq.Skill.Attr att in skill.attrs)
            {
                Console.WriteLine($"skill : {skill.id}, {skill.level}, {skill.duration},{att.attrNum}");
            }
        }
    }

}


