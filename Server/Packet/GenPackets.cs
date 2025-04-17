using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using ServerCore;

public enum PacketID
{
	C_RoomEnter = 1,
	S_RoomEnter = 2,
	C_RoomExit = 3,
	C_CreateRoom = 4,
	S_RoomList = 5,
	C_RoomList = 6,
	
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

class RoomInfoPacket : IDataPacket
{
	public int roomId;
	public int maxCount;
	public int currentCount;

	public ushort Deserialize(ArraySegment<byte> segment, int offset)
	{
		ushort count = (ushort)offset;
		count += PacketUtility.ReadIntData(segment, count, out roomId);
		count += PacketUtility.ReadIntData(segment, count, out maxCount);
		count += PacketUtility.ReadIntData(segment, count, out currentCount);
		return (ushort)(count - offset);
	}

	public ushort Serialize(ArraySegment<byte> segment, int offset)
	{
		ushort count = (ushort)offset;
		count += PacketUtility.AppendIntData(this.roomId, segment, count);
		count += PacketUtility.AppendIntData(this.maxCount, segment, count);
		count += PacketUtility.AppendIntData(this.currentCount, segment, count);
		return (ushort)(count - offset);
	}
}

class C_RoomEnter : IPacket
{
	public string name;
	public int roomId;

	public ushort Protocol { get { return (ushort)PacketID.C_RoomEnter; } }

	public void Deserialize(ArraySegment<byte> segment)
	{
		ushort count = 0;

		count += sizeof(ushort);
		count += sizeof(ushort);
		count += PacketUtility.ReadStringData(segment, count, out name);
		count += PacketUtility.ReadIntData(segment, count, out roomId);
	}

	public ArraySegment<byte> Serialize()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		ushort count = 0;

		count += sizeof(ushort);
		count += PacketUtility.AppendUshortData(this.Protocol, segment, count);
		count += PacketUtility.AppendStringData(this.name, segment, count);
		count += PacketUtility.AppendIntData(this.roomId, segment, count);
		PacketUtility.AppendUshortData(count, segment, 0);
		return SendBufferHelper.Close(count);
	}
}

class S_RoomEnter : IPacket
{
	

	public ushort Protocol { get { return (ushort)PacketID.S_RoomEnter; } }

	public void Deserialize(ArraySegment<byte> segment)
	{
		ushort count = 0;

		count += sizeof(ushort);
		count += sizeof(ushort);
		
	}

	public ArraySegment<byte> Serialize()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		ushort count = 0;

		count += sizeof(ushort);
		count += PacketUtility.AppendUshortData(this.Protocol, segment, count);
		
		PacketUtility.AppendUshortData(count, segment, 0);
		return SendBufferHelper.Close(count);
	}
}

class C_RoomExit : IPacket
{
	

	public ushort Protocol { get { return (ushort)PacketID.C_RoomExit; } }

	public void Deserialize(ArraySegment<byte> segment)
	{
		ushort count = 0;

		count += sizeof(ushort);
		count += sizeof(ushort);
		
	}

	public ArraySegment<byte> Serialize()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		ushort count = 0;

		count += sizeof(ushort);
		count += PacketUtility.AppendUshortData(this.Protocol, segment, count);
		
		PacketUtility.AppendUshortData(count, segment, 0);
		return SendBufferHelper.Close(count);
	}
}

class C_CreateRoom : IPacket
{
	

	public ushort Protocol { get { return (ushort)PacketID.C_CreateRoom; } }

	public void Deserialize(ArraySegment<byte> segment)
	{
		ushort count = 0;

		count += sizeof(ushort);
		count += sizeof(ushort);
		
	}

	public ArraySegment<byte> Serialize()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		ushort count = 0;

		count += sizeof(ushort);
		count += PacketUtility.AppendUshortData(this.Protocol, segment, count);
		
		PacketUtility.AppendUshortData(count, segment, 0);
		return SendBufferHelper.Close(count);
	}
}

class S_RoomList : IPacket
{
	public List<RoomInfoPacket> roomInfos;

	public ushort Protocol { get { return (ushort)PacketID.S_RoomList; } }

	public void Deserialize(ArraySegment<byte> segment)
	{
		ushort count = 0;

		count += sizeof(ushort);
		count += sizeof(ushort);
		count += PacketUtility.ReadListData(segment, count, out roomInfos);
	}

	public ArraySegment<byte> Serialize()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		ushort count = 0;

		count += sizeof(ushort);
		count += PacketUtility.AppendUshortData(this.Protocol, segment, count);
		count += PacketUtility.AppendListData(this.roomInfos, segment, count);
		PacketUtility.AppendUshortData(count, segment, 0);
		return SendBufferHelper.Close(count);
	}
}

class C_RoomList : IPacket
{
	

	public ushort Protocol { get { return (ushort)PacketID.C_RoomList; } }

	public void Deserialize(ArraySegment<byte> segment)
	{
		ushort count = 0;

		count += sizeof(ushort);
		count += sizeof(ushort);
		
	}

	public ArraySegment<byte> Serialize()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		ushort count = 0;

		count += sizeof(ushort);
		count += PacketUtility.AppendUshortData(this.Protocol, segment, count);
		
		PacketUtility.AppendUshortData(count, segment, 0);
		return SendBufferHelper.Close(count);
	}
}

