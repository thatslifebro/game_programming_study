using System.Text;
using ServerCore;
using System.Net;
public enum PacketId
{
    C_PlayerInfoReq = 1,
    S_Test = 2,
    
}

interface IPacket
{
	ushort Protocol { get; }
	void Deserialize(ArraySegment<byte> segment);
	ArraySegment<byte> Serialize();
}


class C_PlayerInfoReq : IPacket
{
    public byte testByte;
	public long playerId;
	public string name;
	public class Skill
	{
	    public int id;
		public short level;
		public float duration;
		public class Attr
		{
		    public int attrNum;
		
		    public bool Write(Span<byte> s,ref ushort count)
		    {
		        bool success = true;
		        success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length-count), this.attrNum);
				count += sizeof(int);
		        return success;
		    }
		
		    public void Read(ReadOnlySpan<byte>s,ref ushort count)
		    {
		        this.attrNum = BitConverter.ToInt32(s.Slice(count,s.Length-count));
				count += sizeof(int);
		    }
		
		}
		public List<Attr> attrs = new List<Attr>();
	
	    public bool Write(Span<byte> s,ref ushort count)
	    {
	        bool success = true;
	        success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length-count), this.id);
			count += sizeof(int);
			success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length-count), this.level);
			count += sizeof(short);
			success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length-count), this.duration);
			count += sizeof(float);
			success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), (ushort)this.attrs.Count);
			count += sizeof(ushort);
			foreach(Attr attr in attrs)
			{
			    success &= attr.Write(s, ref count);
			}
	        return success;
	    }
	
	    public void Read(ReadOnlySpan<byte>s,ref ushort count)
	    {
	        this.id = BitConverter.ToInt32(s.Slice(count,s.Length-count));
			count += sizeof(int);
			this.level = BitConverter.ToInt16(s.Slice(count,s.Length-count));
			count += sizeof(short);
			this.duration = BitConverter.ToSingle(s.Slice(count,s.Length-count));
			count += sizeof(float);
			attrs.Clear();
			ushort attrLen = BitConverter.ToUInt16(s.Slice(count, s.Length - count));
			count += sizeof(ushort);
			for (int i=0;i < attrLen;i++)
			{
			    Attr attr= new Attr();
			    attr.Read(s, ref count);
			    attrs.Add(attr);
			}
	    }
	
	}
	public List<Skill> skills = new List<Skill>();

    public ushort Protocol { get { return (ushort)PacketId.C_PlayerInfoReq; } }

    public ArraySegment<byte> Serialize()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);

        ushort count = 0;
        bool success = true;

        Span<byte> s = new Span<byte>(segment.Array, segment.Offset, segment.Count);
        count += sizeof(ushort);
        success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length-count), (ushort)PacketId.C_PlayerInfoReq);
        count += sizeof(ushort);
        segment.Array[segment.Offset + count] = (byte)this.testByte;
		count += sizeof(byte);
		success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length-count), this.playerId);
		count += sizeof(long);
		ushort nameLen = (ushort)Encoding.Unicode.GetBytes(this.name, 0, this.name.Length, segment.Array, segment.Offset+count+sizeof(ushort));
		success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), nameLen);
		count += sizeof(ushort);
		count += nameLen;
		success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), (ushort)this.skills.Count);
		count += sizeof(ushort);
		foreach(Skill skill in skills)
		{
		    success &= skill.Write(s, ref count);
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
        this.testByte = (byte)segment.Array[segment.Offset + count];
		count += sizeof(byte);
		this.playerId = BitConverter.ToInt64(s.Slice(count,s.Length-count));
		count += sizeof(long);
		ushort nameLen = BitConverter.ToUInt16(s.Slice(count, s.Length - count));
		count += sizeof(ushort);
		this.name = Encoding.Unicode.GetString(s.Slice(count,nameLen));
		count += nameLen;
		skills.Clear();
		ushort skillLen = BitConverter.ToUInt16(s.Slice(count, s.Length - count));
		count += sizeof(ushort);
		for (int i=0;i < skillLen;i++)
		{
		    Skill skill= new Skill();
		    skill.Read(s, ref count);
		    skills.Add(skill);
		}
    }
}


class S_Test : IPacket
{
    public long testLong;
	public string name;
	public class Skill
	{
	    public int id;
		public short level;
		public float duration;
	
	    public bool Write(Span<byte> s,ref ushort count)
	    {
	        bool success = true;
	        success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length-count), this.id);
			count += sizeof(int);
			success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length-count), this.level);
			count += sizeof(short);
			success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length-count), this.duration);
			count += sizeof(float);
	        return success;
	    }
	
	    public void Read(ReadOnlySpan<byte>s,ref ushort count)
	    {
	        this.id = BitConverter.ToInt32(s.Slice(count,s.Length-count));
			count += sizeof(int);
			this.level = BitConverter.ToInt16(s.Slice(count,s.Length-count));
			count += sizeof(short);
			this.duration = BitConverter.ToSingle(s.Slice(count,s.Length-count));
			count += sizeof(float);
	    }
	
	}
	public List<Skill> skills = new List<Skill>();

    public ushort Protocol { get { return (ushort)PacketId.S_Test; } }

    public ArraySegment<byte> Serialize()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);

        ushort count = 0;
        bool success = true;

        Span<byte> s = new Span<byte>(segment.Array, segment.Offset, segment.Count);
        count += sizeof(ushort);
        success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length-count), (ushort)PacketId.S_Test);
        count += sizeof(ushort);
        success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length-count), this.testLong);
		count += sizeof(long);
		ushort nameLen = (ushort)Encoding.Unicode.GetBytes(this.name, 0, this.name.Length, segment.Array, segment.Offset+count+sizeof(ushort));
		success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), nameLen);
		count += sizeof(ushort);
		count += nameLen;
		success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), (ushort)this.skills.Count);
		count += sizeof(ushort);
		foreach(Skill skill in skills)
		{
		    success &= skill.Write(s, ref count);
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
        this.testLong = BitConverter.ToInt64(s.Slice(count,s.Length-count));
		count += sizeof(long);
		ushort nameLen = BitConverter.ToUInt16(s.Slice(count, s.Length - count));
		count += sizeof(ushort);
		this.name = Encoding.Unicode.GetString(s.Slice(count,nameLen));
		count += nameLen;
		skills.Clear();
		ushort skillLen = BitConverter.ToUInt16(s.Slice(count, s.Length - count));
		count += sizeof(ushort);
		for (int i=0;i < skillLen;i++)
		{
		    Skill skill= new Skill();
		    skill.Read(s, ref count);
		    skills.Add(skill);
		}
    }
}



