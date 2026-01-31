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
	public VectorPacket(){}

	public float x = default;
	public float y = default;
	public float z = default;

    public void Serialize<T>(ref T serializer) where T : struct, IPacketSerializer
    {
        serializer.Serialize(ref x);
		serializer.Serialize(ref y);
		serializer.Serialize(ref z);
    }
}

public struct QuaternionPacket : IDataPacket
{
	public QuaternionPacket(){}

	public float x = default;
	public float y = default;
	public float z = default;
	public float w = default;

    public void Serialize<T>(ref T serializer) where T : struct, IPacketSerializer
    {
        serializer.Serialize(ref x);
		serializer.Serialize(ref y);
		serializer.Serialize(ref z);
		serializer.Serialize(ref w);
    }
}

