using System;
namespace PacketGenerator
{
	class PacketFormat
	{
        //0 : packet이름 1: 멤버변수 2: 멤버변수 serialize  3: 멤버변수 desrialize
		public static string packetFormat =
@"
class {0}
{{
    {1}

    public ArraySegment<byte> Serialize()
    {{
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);

        ushort count = 0;
        bool success = true;

        Span<byte> s = new Span<byte>(segment.Array, segment.Offset, segment.Count);
        count += sizeof(ushort);
        success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length-count), (ushort)PacketeId.{0});
        count += sizeof(ushort);
        {2}
        success &= BitConverter.TryWriteBytes(s, count);

        if (success == false)
            return null;

        return SendBufferHelper.Close(count);
    }}

    public void Deserialize(ArraySegment<byte> segment)
    {{
        ushort count = 0;

        ReadOnlySpan<byte> s = new ReadOnlySpan<byte>(segment.Array, segment.Offset, segment.Count);
        count += sizeof(ushort);
        count += sizeof(ushort);
        {3}
    }}
}}

"
;
        //0 : 변수형식 1: 변수이름
        public static string memberFormat =
@"public {0} {1};";

        //0: 리스트이름 대문자 1: 리스트이름 소문자 2: member 변수 3: 멤버변수 serialize  4: 멤버변수 desrialize
        public static string memberListFormat =
@"public struct {0}
{{
    {2}

    public bool Write(Span<byte> s,ref ushort count)
    {{
        bool success = true;
        {3}
        return success;
    }}

    public void Read(ReadOnlySpan<byte>s,ref ushort count)
    {{
        {4}
    }}

}}
public List<{0}> {1}s = new List<{0}>();";

        // 0 : 변수 이름 1: 변수형식
        public static string serializeFormat =
@"success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length-count), this.{0});
count += sizeof({1});";

        // 0: 변수 이름 1 :ToINT32 이런거 2 : 변수 형식
        public static string deserializeFormat =
@"this.{0} = BitConverter.{1}(s.Slice(count,s.Length-count));
count += sizeof({2});";

         //0 변수이름
        public static string deserializeStringFormat =
@"ushort {0}Len = BitConverter.ToUInt16(s.Slice(count, s.Length - count));
count += sizeof(ushort);
this.{0} = Encoding.Unicode.GetString(s.Slice(count,{0}Len));
count += {0}Len;";

        //0 변수이름 
        public static string serializeStringFormat =
@"ushort {0}Len = (ushort)Encoding.Unicode.GetBytes(this.{0}, 0, this.{0}.Length, segment.Array, segment.Offset+count+sizeof(ushort));
success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), {0}Len);
count += sizeof(ushort);
count += {0}Len;";

        //0: list이름 대문자  1: list 이름 소문자 
        public static string serializeListFormat =
@"success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), (ushort)this.{1}s.Count);
count += sizeof(ushort);
foreach({0} {1} in {1}s)
{{
    success &= {1}.Write(s, ref count);
}}";
        //0: list이름 대문자  1: list 이름 소문자 
        public static string deserializeListFormat =
@"{1}s.Clear();
ushort {1}Len = BitConverter.ToUInt16(s.Slice(count, s.Length - count));
count += sizeof(ushort);
for (int i=0;i < {1}Len;i++)
{{
    {0} {1}= new {0}();
    {1}.Read(s, ref count);
    {1}s.Add({1});
}}";
    }

        
}

