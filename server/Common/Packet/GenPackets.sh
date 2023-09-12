#!/bin/bash

../../PacketGenerator/bin/PacketGenerator ../../PacketGenerator/PDL.xml

cp ./GenPackets.cs ../../DummyClient/Packet/GenPackets.cs

cp ./GenPackets.cs ../../server/Packet/GenPackets.cs