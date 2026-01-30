using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using ServerCore;
using ServerCore.Serializers;

public enum PacketID
{
	Test = 1,
	
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

public struct Test : IPacket
{
	public Test(){}

	public List<VectorPacket> lists = default;
	public float asd = default;

	public ushort Protocol => _protocol;
	private ushort _protocol = (ushort)PacketID.Test;

    public void Serialize<T>(ref T serializer) where T : struct, IPacketSerializer
	{
		serializer.Serialize(ref _protocol);
		serializer.Serialize(ref lists);
		serializer.Serialize(ref asd);
		serializer.SerializeOffset();
	}
}

