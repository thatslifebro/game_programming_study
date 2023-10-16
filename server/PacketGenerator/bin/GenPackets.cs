using System.Text;
using ServerCore;
using System.Net;
using System;
using System.Collections.Generic;

public enum PacketId
{
    C_RequestMatching = 1,
    S_ResponseMatching = 2,
    C_LeaveGame = 3,
    S_BroadcastLeaveGame = 4,
    S_PlayerList = 5,
    C_Move = 6,
    S_BroadcastMove = 7,
    S_GameOver = 8,
    
}

public interface IPacket
{
	ushort Protocol { get; }
	void Deserialize(ArraySegment<byte> segment);
	ArraySegment<byte> Serialize();
}


public class C_RequestMatching : IPacket
{
    

    public ushort Protocol { get { return (ushort)PacketId.C_RequestMatching; } }

    public ArraySegment<byte> Serialize()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);

        ushort count = 0;
        bool success = true;

        Span<byte> s = new Span<byte>(segment.Array, segment.Offset, segment.Count);
        count += sizeof(ushort);
        success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length-count), (ushort)PacketId.C_RequestMatching);
        count += sizeof(ushort);
        
        success &= BitConverter.TryWriteBytes(s, count);

        if (success == false)
            return null;

        return SendBufferHelper.Close(count);
    }

    public void Deserialize(ArraySegment<byte> segment)
    {
        ushort count = 0;

        ReadOnlySpan<byte> s = new ReadOnlySpan<byte>(segment.Array, segment.Offset, segment.Count);
        count += sizeof(ushort);
        count += sizeof(ushort);
        
    }
}


public class S_ResponseMatching : IPacket
{
    public int otherPlayerId;
	public bool amIWhite;

    public ushort Protocol { get { return (ushort)PacketId.S_ResponseMatching; } }

    public ArraySegment<byte> Serialize()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);

        ushort count = 0;
        bool success = true;

        Span<byte> s = new Span<byte>(segment.Array, segment.Offset, segment.Count);
        count += sizeof(ushort);
        success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length-count), (ushort)PacketId.S_ResponseMatching);
        count += sizeof(ushort);
        success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length-count), this.otherPlayerId);
		count += sizeof(int);
		success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length-count), this.amIWhite);
		count += sizeof(bool);
        success &= BitConverter.TryWriteBytes(s, count);

        if (success == false)
            return null;

        return SendBufferHelper.Close(count);
    }

    public void Deserialize(ArraySegment<byte> segment)
    {
        ushort count = 0;

        ReadOnlySpan<byte> s = new ReadOnlySpan<byte>(segment.Array, segment.Offset, segment.Count);
        count += sizeof(ushort);
        count += sizeof(ushort);
        this.otherPlayerId = BitConverter.ToInt32(s.Slice(count,s.Length-count));
		count += sizeof(int);
		this.amIWhite = BitConverter.ToBoolean(s.Slice(count,s.Length-count));
		count += sizeof(bool);
    }
}


public class C_LeaveGame : IPacket
{
    

    public ushort Protocol { get { return (ushort)PacketId.C_LeaveGame; } }

    public ArraySegment<byte> Serialize()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);

        ushort count = 0;
        bool success = true;

        Span<byte> s = new Span<byte>(segment.Array, segment.Offset, segment.Count);
        count += sizeof(ushort);
        success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length-count), (ushort)PacketId.C_LeaveGame);
        count += sizeof(ushort);
        
        success &= BitConverter.TryWriteBytes(s, count);

        if (success == false)
            return null;

        return SendBufferHelper.Close(count);
    }

    public void Deserialize(ArraySegment<byte> segment)
    {
        ushort count = 0;

        ReadOnlySpan<byte> s = new ReadOnlySpan<byte>(segment.Array, segment.Offset, segment.Count);
        count += sizeof(ushort);
        count += sizeof(ushort);
        
    }
}


public class S_BroadcastLeaveGame : IPacket
{
    

    public ushort Protocol { get { return (ushort)PacketId.S_BroadcastLeaveGame; } }

    public ArraySegment<byte> Serialize()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);

        ushort count = 0;
        bool success = true;

        Span<byte> s = new Span<byte>(segment.Array, segment.Offset, segment.Count);
        count += sizeof(ushort);
        success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length-count), (ushort)PacketId.S_BroadcastLeaveGame);
        count += sizeof(ushort);
        
        success &= BitConverter.TryWriteBytes(s, count);

        if (success == false)
            return null;

        return SendBufferHelper.Close(count);
    }

    public void Deserialize(ArraySegment<byte> segment)
    {
        ushort count = 0;

        ReadOnlySpan<byte> s = new ReadOnlySpan<byte>(segment.Array, segment.Offset, segment.Count);
        count += sizeof(ushort);
        count += sizeof(ushort);
        
    }
}


public class S_PlayerList : IPacket
{
    public class Player
	{
	    public bool isSelf;
		public bool isInGame;
		public int playerId;
	
	    public bool Write(Span<byte> s,ref ushort count)
	    {
	        bool success = true;
	        success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length-count), this.isSelf);
			count += sizeof(bool);
			success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length-count), this.isInGame);
			count += sizeof(bool);
			success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length-count), this.playerId);
			count += sizeof(int);
	        return success;
	    }
	
	    public void Read(ReadOnlySpan<byte>s,ref ushort count)
	    {
	        this.isSelf = BitConverter.ToBoolean(s.Slice(count,s.Length-count));
			count += sizeof(bool);
			this.isInGame = BitConverter.ToBoolean(s.Slice(count,s.Length-count));
			count += sizeof(bool);
			this.playerId = BitConverter.ToInt32(s.Slice(count,s.Length-count));
			count += sizeof(int);
	    }
	
	}
	public List<Player> players = new List<Player>();

    public ushort Protocol { get { return (ushort)PacketId.S_PlayerList; } }

    public ArraySegment<byte> Serialize()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);

        ushort count = 0;
        bool success = true;

        Span<byte> s = new Span<byte>(segment.Array, segment.Offset, segment.Count);
        count += sizeof(ushort);
        success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length-count), (ushort)PacketId.S_PlayerList);
        count += sizeof(ushort);
        success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), (ushort)this.players.Count);
		count += sizeof(ushort);
		foreach(Player player in players)
		{
		    success &= player.Write(s, ref count);
		}
        success &= BitConverter.TryWriteBytes(s, count);

        if (success == false)
            return null;

        return SendBufferHelper.Close(count);
    }

    public void Deserialize(ArraySegment<byte> segment)
    {
        ushort count = 0;

        ReadOnlySpan<byte> s = new ReadOnlySpan<byte>(segment.Array, segment.Offset, segment.Count);
        count += sizeof(ushort);
        count += sizeof(ushort);
        players.Clear();
		ushort playerLen = BitConverter.ToUInt16(s.Slice(count, s.Length - count));
		count += sizeof(ushort);
		for (int i=0;i < playerLen;i++)
		{
		    Player player= new Player();
		    player.Read(s, ref count);
		    players.Add(player);
		}
    }
}


public class C_Move : IPacket
{
    public int prevX;
	public int prevY;
	public int nextX;
	public int nextY;
	public int promotion;

    public ushort Protocol { get { return (ushort)PacketId.C_Move; } }

    public ArraySegment<byte> Serialize()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);

        ushort count = 0;
        bool success = true;

        Span<byte> s = new Span<byte>(segment.Array, segment.Offset, segment.Count);
        count += sizeof(ushort);
        success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length-count), (ushort)PacketId.C_Move);
        count += sizeof(ushort);
        success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length-count), this.prevX);
		count += sizeof(int);
		success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length-count), this.prevY);
		count += sizeof(int);
		success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length-count), this.nextX);
		count += sizeof(int);
		success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length-count), this.nextY);
		count += sizeof(int);
		success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length-count), this.promotion);
		count += sizeof(int);
        success &= BitConverter.TryWriteBytes(s, count);

        if (success == false)
            return null;

        return SendBufferHelper.Close(count);
    }

    public void Deserialize(ArraySegment<byte> segment)
    {
        ushort count = 0;

        ReadOnlySpan<byte> s = new ReadOnlySpan<byte>(segment.Array, segment.Offset, segment.Count);
        count += sizeof(ushort);
        count += sizeof(ushort);
        this.prevX = BitConverter.ToInt32(s.Slice(count,s.Length-count));
		count += sizeof(int);
		this.prevY = BitConverter.ToInt32(s.Slice(count,s.Length-count));
		count += sizeof(int);
		this.nextX = BitConverter.ToInt32(s.Slice(count,s.Length-count));
		count += sizeof(int);
		this.nextY = BitConverter.ToInt32(s.Slice(count,s.Length-count));
		count += sizeof(int);
		this.promotion = BitConverter.ToInt32(s.Slice(count,s.Length-count));
		count += sizeof(int);
    }
}


public class S_BroadcastMove : IPacket
{
    public int playerId;
	public int prevX;
	public int prevY;
	public int nextX;
	public int nextY;

    public ushort Protocol { get { return (ushort)PacketId.S_BroadcastMove; } }

    public ArraySegment<byte> Serialize()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);

        ushort count = 0;
        bool success = true;

        Span<byte> s = new Span<byte>(segment.Array, segment.Offset, segment.Count);
        count += sizeof(ushort);
        success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length-count), (ushort)PacketId.S_BroadcastMove);
        count += sizeof(ushort);
        success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length-count), this.playerId);
		count += sizeof(int);
		success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length-count), this.prevX);
		count += sizeof(int);
		success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length-count), this.prevY);
		count += sizeof(int);
		success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length-count), this.nextX);
		count += sizeof(int);
		success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length-count), this.nextY);
		count += sizeof(int);
        success &= BitConverter.TryWriteBytes(s, count);

        if (success == false)
            return null;

        return SendBufferHelper.Close(count);
    }

    public void Deserialize(ArraySegment<byte> segment)
    {
        ushort count = 0;

        ReadOnlySpan<byte> s = new ReadOnlySpan<byte>(segment.Array, segment.Offset, segment.Count);
        count += sizeof(ushort);
        count += sizeof(ushort);
        this.playerId = BitConverter.ToInt32(s.Slice(count,s.Length-count));
		count += sizeof(int);
		this.prevX = BitConverter.ToInt32(s.Slice(count,s.Length-count));
		count += sizeof(int);
		this.prevY = BitConverter.ToInt32(s.Slice(count,s.Length-count));
		count += sizeof(int);
		this.nextX = BitConverter.ToInt32(s.Slice(count,s.Length-count));
		count += sizeof(int);
		this.nextY = BitConverter.ToInt32(s.Slice(count,s.Length-count));
		count += sizeof(int);
    }
}


public class S_GameOver : IPacket
{
    public bool Draw;
	public bool youWin;
	public bool youLose;

    public ushort Protocol { get { return (ushort)PacketId.S_GameOver; } }

    public ArraySegment<byte> Serialize()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);

        ushort count = 0;
        bool success = true;

        Span<byte> s = new Span<byte>(segment.Array, segment.Offset, segment.Count);
        count += sizeof(ushort);
        success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length-count), (ushort)PacketId.S_GameOver);
        count += sizeof(ushort);
        success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length-count), this.Draw);
		count += sizeof(bool);
		success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length-count), this.youWin);
		count += sizeof(bool);
		success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length-count), this.youLose);
		count += sizeof(bool);
        success &= BitConverter.TryWriteBytes(s, count);

        if (success == false)
            return null;

        return SendBufferHelper.Close(count);
    }

    public void Deserialize(ArraySegment<byte> segment)
    {
        ushort count = 0;

        ReadOnlySpan<byte> s = new ReadOnlySpan<byte>(segment.Array, segment.Offset, segment.Count);
        count += sizeof(ushort);
        count += sizeof(ushort);
        this.Draw = BitConverter.ToBoolean(s.Slice(count,s.Length-count));
		count += sizeof(bool);
		this.youWin = BitConverter.ToBoolean(s.Slice(count,s.Length-count));
		count += sizeof(bool);
		this.youLose = BitConverter.ToBoolean(s.Slice(count,s.Length-count));
		count += sizeof(bool);
    }
}



