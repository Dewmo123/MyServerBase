using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using ServerCore;

public enum PacketID
{
	C_Chat = 1,
	S_Chat = 2,
	
}

class VectorPacket : IDataPacket
{
	public float x;
	public float y;
	public float z;

	public ushort Deserialize(ArraySegment<byte> segment, int offset)
	{
		ushort count = (ushort)offset;
		count += PacketUtility.ReadFloatData(segment, count, out x);
		count += PacketUtility.ReadFloatData(segment, count, out y);
		count += PacketUtility.ReadFloatData(segment, count, out z);
		return (ushort)(count - offset);
	}

	public ushort Serialize(ArraySegment<byte> segment, int offset)
	{
		ushort count = (ushort)offset;
		count += PacketUtility.AppendFloatData(this.x, segment, count);
		count += PacketUtility.AppendFloatData(this.y, segment, count);
		count += PacketUtility.AppendFloatData(this.z, segment, count);
		return (ushort)(count - offset);
	}
}

class C_Chat : IPacket
{
	public string chat;

	public ushort Protocol { get { return (ushort)PacketID.C_Chat; } }

	public void Deserialize(ArraySegment<byte> segment)
	{
		ushort count = 0;

		count += sizeof(ushort);
		count += sizeof(ushort);
		count += PacketUtility.ReadStringData(segment, count, out chat);
	}

	public ArraySegment<byte> Serialize()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		ushort count = 0;

		count += sizeof(ushort);
		count += PacketUtility.AppendUshortData(count, segment, this.Protocol);
		count += PacketUtility.AppendStringData(this.chat, segment, count);
		PacketUtility.AppendUshortData(count, segment, 0);
		return SendBufferHelper.Close(count);
	}
}

class S_Chat : IPacket
{
	public int playerId;
	public string chat;
	public VectorPacket Pos;

	public ushort Protocol { get { return (ushort)PacketID.S_Chat; } }

	public void Deserialize(ArraySegment<byte> segment)
	{
		ushort count = 0;

		count += sizeof(ushort);
		count += sizeof(ushort);
		count += PacketUtility.ReadIntData(segment, count, out playerId);
		count += PacketUtility.ReadStringData(segment, count, out chat);
		count += PacketUtility.ReadDataPacketData(segment, count, out Pos);
	}

	public ArraySegment<byte> Serialize()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		ushort count = 0;

		count += sizeof(ushort);
		count += PacketUtility.AppendUshortData(count, segment, this.Protocol);
		count += PacketUtility.AppendIntData(this.playerId, segment, count);
		count += PacketUtility.AppendStringData(this.chat, segment, count);
		count += PacketUtility.AppendDataPacketData(this.Pos, segment, count);
		PacketUtility.AppendUshortData(count, segment, 0);
		return SendBufferHelper.Close(count);
	}
}

