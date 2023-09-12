using System;
using System.Xml;

namespace PacketGenerator
{
    class Program
    {
        static string genPackets;

        static ushort packetId;
        static string packetEnums;
        
        static void Main(string[] args)
        {
            XmlReaderSettings settings = new XmlReaderSettings()
            {
                IgnoreComments = true,
                IgnoreWhitespace = true,
            };

            using(XmlReader r = XmlReader.Create("PDL.xml", settings))
            {
                r.MoveToContent();

                while (r.Read())
                {
                    if (r.Depth == 1  && r.NodeType == XmlNodeType.Element)
                    {
                        ParsePacket(r);
                    }
                    //Console.WriteLine(r.Name+" " + r["name"]);

                    string fileText = string.Format(PacketFormat.fileFormat, packetEnums, genPackets);

                    File.WriteAllText("GenPackets.cs", fileText);
                }
            }

        }

        public static void ParsePacket(XmlReader r)
        {
            if (r.NodeType == XmlNodeType.EndElement) return;
            if (r.Name.ToLower() != "packet")
            {
                Console.WriteLine("invalid packet node type");
                return;
            }

            string packetName = r["name"];
            if (string.IsNullOrEmpty(packetName))
            {
                Console.WriteLine("Packet without name");
                return;
            }

            Tuple<string,string,string> t = ParseMembers(r);

            genPackets += string.Format(PacketFormat.packetFormat, packetName, t.Item1,t.Item2,t.Item3);
            packetEnums += string.Format(PacketFormat.packetEnumFormat, packetName, ++packetId);
            
        }

        // 1: 멤버변수 2: 멤버변수 serialize  3: 멤버변수 desrialize
        public static Tuple<string,string,string> ParseMembers(XmlReader r)
        {
            string packetName = r["name"];

            string memberCode = "";
            string serializeCode = "";
            string deserializeCode = "";

            int depth = r.Depth + 1;
            while (r.Read())
            {
                if (r.Depth != depth) break;

                string memberName = r["name"];

                if (string.IsNullOrEmpty(memberName))
                {
                    Console.WriteLine("member without name");
                    return null;
                }

                //엔터 작업 
                if (string.IsNullOrEmpty(memberCode) == false)
                    memberCode += Environment.NewLine;
                if (string.IsNullOrEmpty(serializeCode) == false)
                    serializeCode += Environment.NewLine;
                if (string.IsNullOrEmpty(deserializeCode) == false)
                    deserializeCode += Environment.NewLine;


                string memberType = r.Name.ToLower();
                switch (memberType)
                {
                    case "byte":
                    case "sbyte":
                        memberCode += string.Format(PacketFormat.memberFormat, memberType, memberName);
                        serializeCode += string.Format(PacketFormat.serializeByteFormat, memberName, memberType);
                        deserializeCode += string.Format(PacketFormat.deserializeByteFormat, memberName, memberType);
                        break;
                    case "bool": 
                    case "short":
                    case "ushort":
                    case "int":
                    case "long":
                    case "float":
                    case "double":
                        memberCode += string.Format(PacketFormat.memberFormat, memberType, memberName);
                        serializeCode += string.Format(PacketFormat.serializeFormat, memberName, memberType);
                        deserializeCode += string.Format(PacketFormat.deserializeFormat, memberName, ToMemberType(memberType), memberType);
                        break;
                    case "string":
                        memberCode += string.Format(PacketFormat.memberFormat, memberType, memberName);
                        serializeCode += string.Format(PacketFormat.serializeStringFormat, memberName);
                        deserializeCode += string.Format(PacketFormat.deserializeStringFormat, memberName);
                        break;
                    case "list":
                        Tuple<string,string,string> t = ParseList(r);
                        memberCode += t.Item1;
                        serializeCode += t.Item2;
                        deserializeCode += t.Item3;

                        break;

                    default:
                        break;
                }

                
            }

            memberCode = memberCode.Replace("\n", "\n\t");
            serializeCode = serializeCode.Replace("\n", "\n\t\t");
            deserializeCode = deserializeCode.Replace("\n", "\n\t\t");

            return new Tuple<string, string, string>(memberCode, serializeCode, deserializeCode);
        }
        public static string ToMemberType(string memberType)
        {
            switch (memberType)
            {
                case "bool":
                    return "ToBoolean";
                case "short":
                    return "ToInt16";
                case "ushort":
                    return "ToUInt16";
                case "int":
                    return "ToInt32";
                case "long":
                    return "ToInt64";
                case "float":
                    return "ToSingle";
                case "double":
                    return "ToDouble";
                default:
                    return "";
            }

        }

        public static string FirstCharacterToUpper(string input)
        {
            if (string.IsNullOrEmpty(input))
                return "";

            return input[0].ToString().ToUpper() + input.Substring(1);
        }

        public static string FirstCharacterToLower(string input)
        {
            if (string.IsNullOrEmpty(input))
                return "";

            return input[0].ToString().ToLower() + input.Substring(1);
        }

        public static Tuple<string, string, string> ParseList(XmlReader r)
        {
            string listName = r["name"];
            if (string.IsNullOrEmpty(listName))
            {
                Console.WriteLine("list without name");
            }

            Tuple<string,string,string> t= ParseMembers(r);

            string memberCode = string.Format(PacketFormat.memberListFormat,
                FirstCharacterToUpper(listName),
                FirstCharacterToLower(listName),
                t.Item1,
                t.Item2,
                t.Item3
                );

            string serializeCode = string.Format(PacketFormat.serializeListFormat,
                FirstCharacterToUpper(listName),
                FirstCharacterToLower(listName));

            string deserializeCode = string.Format(PacketFormat.deserializeListFormat,
                FirstCharacterToUpper(listName),
                FirstCharacterToLower(listName));

            return new Tuple<string, string, string>(memberCode,serializeCode,deserializeCode);
        }
        
    }
}