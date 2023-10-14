using System;
using DummyClient;
using ServerCore;
using UnityEngine;

class PacketHandler
{
    public static void S_ResponseMatchingHandler(PacketSession session, IPacket packet)
    {
        S_ResponseMatching pkt = packet as S_ResponseMatching;
        ServerSession serverSession = session as ServerSession;


        PlayerManager.Instance.EnterGame(pkt);
    }

    public static void S_GameOverHandler(PacketSession session, IPacket packet)
    {
        S_GameOver pkt = packet as S_GameOver;
        ServerSession serverSession = session as ServerSession;

        //PlayerManager.Instance.EnterGame(pkt);
    }
    

    public static void S_BroadcastLeaveGameHandler(PacketSession session, IPacket packet)
    {
        S_BroadcastLeaveGame pkt = packet as S_BroadcastLeaveGame;
        ServerSession serverSession = session as ServerSession;

        //PlayerManager.Instance.LeaveGame(pkt);
    }

    public static void S_PlayerListHandler(PacketSession session, IPacket packet)
    {
        S_PlayerList pkt = packet as S_PlayerList;
        ServerSession serverSession = session as ServerSession;

        PlayerManager.Instance.Add(pkt);
    }

    public static void S_BroadcastMoveHandler(PacketSession session, IPacket packet)
    {
        //S_BroadcastMove pkt = packet as S_BroadcastMove;
        //ServerSession serverSession = session as ServerSession;

        //PlayerManager.Instance.Move(pkt);
    }
}
