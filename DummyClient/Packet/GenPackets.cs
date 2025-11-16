using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using ServerCore;
using ServerCore.Serializers;

public enum PacketID
{
	
}

public struct VectorPacket : IDataPacket
{
	public float x;
	public float y;
	public float z;

    public void Serialize<T>(ref T serializer) where T : struct, IPacketSerializer
    {
		serializer.Serialize(ref x);
		serializer.Serialize(ref y);
		serializer.Serialize(ref z);
    }
}

public struct QuaternionPacket : IDataPacket
{
	public float x;
	public float y;
	public float z;
	public float w;

    public void Serialize<T>(ref T serializer) where T : struct, IPacketSerializer
    {
        serializer.Serialize(ref x);
        serializer.Serialize(ref y);
        serializer.Serialize(ref z);
        serializer.Serialize(ref w);
    }
}

