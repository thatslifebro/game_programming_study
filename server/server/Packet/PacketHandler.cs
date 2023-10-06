﻿using System;
using server;
using Server;
using ServerCore;


class PacketHandler
{
	public static void C_LeaveGameHandler(PacketSession session, IPacket packet)
	{
		ClientSession clientSession = session as ClientSession;

		if(clientSession.Room == null)
			return;
		else
		{
			GameRoom room = clientSession.Room;
			room.Push(
				() => room.Leave(clientSession)
				);
		}

    }

    public static void C_MoveHandler(PacketSession session, IPacket packet)
    {
        C_Move movePacket = packet as C_Move;
        ClientSession clientSession = session as ClientSession;

        if (clientSession.Room == null)
            return;
        else
        {
            Console.WriteLine($"{movePacket.posX},{movePacket.posZ}");
            GameRoom room = clientSession.Room;
            room.Push(
                () => room.Move(clientSession, movePacket)
                );
        }

    }

}


