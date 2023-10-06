#!/bin/bash

../../PacketGenerator/bin/PacketGenerator ../../PacketGenerator/PDL.xml

cp ./GenPackets.cs ../../DummyClient/Packet/GenPackets.cs

cp ./GenPackets.cs /Users/sy/Desktop/unitygame/test/Assets/Scripts/Packet/GenPackets.cs

cp ./GenPackets.cs ../../server/Packet/GenPackets.cs

cp ./ClientPacketManager.cs ../../DummyClient/Packet/ClientPacketManager.cs

cp ./ClientPacketManager.cs /Users/sy/Desktop/unitygame/test/Assets/Scripts/Packet/ClientPacketManager.cs

cp ./ServerPacketManager.cs ../../server/Packet/ServerPacketManager.cs
