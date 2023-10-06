using System.Text;
using ServerCore;
using System.Net;
using System;

public enum PacketId
{
    S_BroadcastEnterGame = 1,
    C_LeaveGame = 2,
    S_BroadcastLeaveGame = 3,
    S_PlayerList = 4,
    C_Move = 5,
    S_BroadcastMove = 6,
    
}

public interface IPacket
{
	ushort Protocol { get; }
	void Deserialize(ArraySegment<byte> segment);
	ArraySegment<byte> Serialize();
}


public class S_BroadcastEnterGame : IPacket
{
    public int playerId;
	public float posX;
	public float posY;
	public float posZ;

    public ushort Protocol { get { return (ushort)PacketId.S_BroadcastEnterGame; } }

    public ArraySegment<byte> Serialize()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);

        ushort count = 0;
        bool success = true;

        Span<byte> s = new Span<byte>(segment.Array, segment.Offset, segment.Count);
        count += sizeof(ushort);
        success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length-count), (ushort)PacketId.S_BroadcastEnterGame);
        count += sizeof(ushort);
        success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length-count), this.playerId);
		count += sizeof(int);
		success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length-count), this.posX);
		count += sizeof(float);
		success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length-count), this.posY);
		count += sizeof(float);
		success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length-count), this.posZ);
		count += sizeof(float);
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
		this.posX = BitConverter.ToSingle(s.Slice(count,s.Length-count));
		count += sizeof(float);
		this.posY = BitConverter.ToSingle(s.Slice(count,s.Length-count));
		count += sizeof(float);
		this.posZ = BitConverter.ToSingle(s.Slice(count,s.Length-count));
		count += sizeof(float);
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
    public int playerId;

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
        success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length-count), this.playerId);
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
    }
}


public class S_PlayerList : IPacket
{
    public class Player
	{
	    public bool isSelf;
		public int playerId;
		public float posX;
		public float posY;
		public float posZ;
	
	    public bool Write(Span<byte> s,ref ushort count)
	    {
	        bool success = true;
	        success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length-count), this.isSelf);
			count += sizeof(bool);
			success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length-count), this.playerId);
			count += sizeof(int);
			success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length-count), this.posX);
			count += sizeof(float);
			success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length-count), this.posY);
			count += sizeof(float);
			success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length-count), this.posZ);
			count += sizeof(float);
	        return success;
	    }
	
	    public void Read(ReadOnlySpan<byte>s,ref ushort count)
	    {
	        this.isSelf = BitConverter.ToBoolean(s.Slice(count,s.Length-count));
			count += sizeof(bool);
			this.playerId = BitConverter.ToInt32(s.Slice(count,s.Length-count));
			count += sizeof(int);
			this.posX = BitConverter.ToSingle(s.Slice(count,s.Length-count));
			count += sizeof(float);
			this.posY = BitConverter.ToSingle(s.Slice(count,s.Length-count));
			count += sizeof(float);
			this.posZ = BitConverter.ToSingle(s.Slice(count,s.Length-count));
			count += sizeof(float);
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
    public float posX;
	public float posY;
	public float posZ;

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
        success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length-count), this.posX);
		count += sizeof(float);
		success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length-count), this.posY);
		count += sizeof(float);
		success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length-count), this.posZ);
		count += sizeof(float);
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
        this.posX = BitConverter.ToSingle(s.Slice(count,s.Length-count));
		count += sizeof(float);
		this.posY = BitConverter.ToSingle(s.Slice(count,s.Length-count));
		count += sizeof(float);
		this.posZ = BitConverter.ToSingle(s.Slice(count,s.Length-count));
		count += sizeof(float);
    }
}


public class S_BroadcastMove : IPacket
{
    public int playerId;
	public float posX;
	public float posY;
	public float posZ;

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
		success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length-count), this.posX);
		count += sizeof(float);
		success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length-count), this.posY);
		count += sizeof(float);
		success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length-count), this.posZ);
		count += sizeof(float);
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
		this.posX = BitConverter.ToSingle(s.Slice(count,s.Length-count));
		count += sizeof(float);
		this.posY = BitConverter.ToSingle(s.Slice(count,s.Length-count));
		count += sizeof(float);
		this.posZ = BitConverter.ToSingle(s.Slice(count,s.Length-count));
		count += sizeof(float);
    }
}



