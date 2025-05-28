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
	S_RoomExit = 4,
	C_CreateRoom = 5,
	S_RoomList = 6,
	C_RoomList = 7,
	S_TestText = 8,
	S_EnterRoomFirst = 9,
	S_UpdateInfos = 10,
	C_UpdateLocation = 11,
	S_TeamInfos = 12,
	S_UpdateLocations = 13,
	C_ShootReq = 14,
	
}

public struct VectorPacket : IDataPacket
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

public struct QuaternionPacket : IDataPacket
{
	public float x;
	public float y;
	public float z;
	public float w;

	public ushort Deserialize(ArraySegment<byte> segment, int offset)
	{
		ushort count = (ushort)offset;
		count += PacketUtility.ReadFloatData(segment, count, out x);
		count += PacketUtility.ReadFloatData(segment, count, out y);
		count += PacketUtility.ReadFloatData(segment, count, out z);
		count += PacketUtility.ReadFloatData(segment, count, out w);
		return (ushort)(count - offset);
	}

	public ushort Serialize(ArraySegment<byte> segment, int offset)
	{
		ushort count = (ushort)offset;
		count += PacketUtility.AppendFloatData(this.x, segment, count);
		count += PacketUtility.AppendFloatData(this.y, segment, count);
		count += PacketUtility.AppendFloatData(this.z, segment, count);
		count += PacketUtility.AppendFloatData(this.w, segment, count);
		return (ushort)(count - offset);
	}
}

public struct RoomInfoPacket : IDataPacket
{
	public int roomId;
	public int maxCount;
	public int currentCount;
	public string roomName;

	public ushort Deserialize(ArraySegment<byte> segment, int offset)
	{
		ushort count = (ushort)offset;
		count += PacketUtility.ReadIntData(segment, count, out roomId);
		count += PacketUtility.ReadIntData(segment, count, out maxCount);
		count += PacketUtility.ReadIntData(segment, count, out currentCount);
		count += PacketUtility.ReadStringData(segment, count, out roomName);
		return (ushort)(count - offset);
	}

	public ushort Serialize(ArraySegment<byte> segment, int offset)
	{
		ushort count = (ushort)offset;
		count += PacketUtility.AppendIntData(this.roomId, segment, count);
		count += PacketUtility.AppendIntData(this.maxCount, segment, count);
		count += PacketUtility.AppendIntData(this.currentCount, segment, count);
		count += PacketUtility.AppendStringData(this.roomName, segment, count);
		return (ushort)(count - offset);
	}
}

public struct PlayerInfoPacket : IDataPacket
{
	public int index;
	public bool isAiming;
	public int Health;

	public ushort Deserialize(ArraySegment<byte> segment, int offset)
	{
		ushort count = (ushort)offset;
		count += PacketUtility.ReadIntData(segment, count, out index);
		count += PacketUtility.ReadBoolData(segment, count, out isAiming);
		count += PacketUtility.ReadIntData(segment, count, out Health);
		return (ushort)(count - offset);
	}

	public ushort Serialize(ArraySegment<byte> segment, int offset)
	{
		ushort count = (ushort)offset;
		count += PacketUtility.AppendIntData(this.index, segment, count);
		count += PacketUtility.AppendBoolData(this.isAiming, segment, count);
		count += PacketUtility.AppendIntData(this.Health, segment, count);
		return (ushort)(count - offset);
	}
}

public struct LocationInfoPacket : IDataPacket
{
	public int index;
	public int animHash;
	public VectorPacket position;
	public QuaternionPacket rotation;
	public QuaternionPacket gunRotation;

	public ushort Deserialize(ArraySegment<byte> segment, int offset)
	{
		ushort count = (ushort)offset;
		count += PacketUtility.ReadIntData(segment, count, out index);
		count += PacketUtility.ReadIntData(segment, count, out animHash);
		count += PacketUtility.ReadDataPacketData(segment, count, out position);
		count += PacketUtility.ReadDataPacketData(segment, count, out rotation);
		count += PacketUtility.ReadDataPacketData(segment, count, out gunRotation);
		return (ushort)(count - offset);
	}

	public ushort Serialize(ArraySegment<byte> segment, int offset)
	{
		ushort count = (ushort)offset;
		count += PacketUtility.AppendIntData(this.index, segment, count);
		count += PacketUtility.AppendIntData(this.animHash, segment, count);
		count += PacketUtility.AppendDataPacketData(this.position, segment, count);
		count += PacketUtility.AppendDataPacketData(this.rotation, segment, count);
		count += PacketUtility.AppendDataPacketData(this.gunRotation, segment, count);
		return (ushort)(count - offset);
	}
}

public struct SnapshotPacket : IDataPacket
{
	public int index;
	public int animHash;
	public long timestamp;
	public VectorPacket position;
	public QuaternionPacket rotation;
	public QuaternionPacket gunRotation;

	public ushort Deserialize(ArraySegment<byte> segment, int offset)
	{
		ushort count = (ushort)offset;
		count += PacketUtility.ReadIntData(segment, count, out index);
		count += PacketUtility.ReadIntData(segment, count, out animHash);
		count += PacketUtility.ReadLongData(segment, count, out timestamp);
		count += PacketUtility.ReadDataPacketData(segment, count, out position);
		count += PacketUtility.ReadDataPacketData(segment, count, out rotation);
		count += PacketUtility.ReadDataPacketData(segment, count, out gunRotation);
		return (ushort)(count - offset);
	}

	public ushort Serialize(ArraySegment<byte> segment, int offset)
	{
		ushort count = (ushort)offset;
		count += PacketUtility.AppendIntData(this.index, segment, count);
		count += PacketUtility.AppendIntData(this.animHash, segment, count);
		count += PacketUtility.AppendLongData(this.timestamp, segment, count);
		count += PacketUtility.AppendDataPacketData(this.position, segment, count);
		count += PacketUtility.AppendDataPacketData(this.rotation, segment, count);
		count += PacketUtility.AppendDataPacketData(this.gunRotation, segment, count);
		return (ushort)(count - offset);
	}
}

public struct TeamInfoPacket : IDataPacket
{
	public int index;
	public ushort team;

	public ushort Deserialize(ArraySegment<byte> segment, int offset)
	{
		ushort count = (ushort)offset;
		count += PacketUtility.ReadIntData(segment, count, out index);
		count += PacketUtility.ReadUshortData(segment, count, out team);
		return (ushort)(count - offset);
	}

	public ushort Serialize(ArraySegment<byte> segment, int offset)
	{
		ushort count = (ushort)offset;
		count += PacketUtility.AppendIntData(this.index, segment, count);
		count += PacketUtility.AppendUshortData(this.team, segment, count);
		return (ushort)(count - offset);
	}
}

public struct AttackInfoBr : IDataPacket
{
	public int hitPlayerIndex;
	public int attackerIndex;
	public int damage;
	public VectorPacket firePos;
	public VectorPacket direction;

	public ushort Deserialize(ArraySegment<byte> segment, int offset)
	{
		ushort count = (ushort)offset;
		count += PacketUtility.ReadIntData(segment, count, out hitPlayerIndex);
		count += PacketUtility.ReadIntData(segment, count, out attackerIndex);
		count += PacketUtility.ReadIntData(segment, count, out damage);
		count += PacketUtility.ReadDataPacketData(segment, count, out firePos);
		count += PacketUtility.ReadDataPacketData(segment, count, out direction);
		return (ushort)(count - offset);
	}

	public ushort Serialize(ArraySegment<byte> segment, int offset)
	{
		ushort count = (ushort)offset;
		count += PacketUtility.AppendIntData(this.hitPlayerIndex, segment, count);
		count += PacketUtility.AppendIntData(this.attackerIndex, segment, count);
		count += PacketUtility.AppendIntData(this.damage, segment, count);
		count += PacketUtility.AppendDataPacketData(this.firePos, segment, count);
		count += PacketUtility.AppendDataPacketData(this.direction, segment, count);
		return (ushort)(count - offset);
	}
}

public class C_RoomEnter : IPacket
{
	public int roomId;

	public ushort Protocol { get { return (ushort)PacketID.C_RoomEnter; } }

	public void Deserialize(ArraySegment<byte> segment)
	{
		ushort count = 0;

		count += sizeof(ushort);
		count += sizeof(ushort);
		count += PacketUtility.ReadIntData(segment, count, out roomId);
	}

	public ArraySegment<byte> Serialize()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		ushort count = 0;

		count += sizeof(ushort);
		count += PacketUtility.AppendUshortData(this.Protocol, segment, count);
		count += PacketUtility.AppendIntData(this.roomId, segment, count);
		PacketUtility.AppendUshortData(count, segment, 0);
		return SendBufferHelper.Close(count);
	}
}

public class S_RoomEnter : IPacket
{
	public LocationInfoPacket newPlayer;

	public ushort Protocol { get { return (ushort)PacketID.S_RoomEnter; } }

	public void Deserialize(ArraySegment<byte> segment)
	{
		ushort count = 0;

		count += sizeof(ushort);
		count += sizeof(ushort);
		count += PacketUtility.ReadDataPacketData(segment, count, out newPlayer);
	}

	public ArraySegment<byte> Serialize()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		ushort count = 0;

		count += sizeof(ushort);
		count += PacketUtility.AppendUshortData(this.Protocol, segment, count);
		count += PacketUtility.AppendDataPacketData(this.newPlayer, segment, count);
		PacketUtility.AppendUshortData(count, segment, 0);
		return SendBufferHelper.Close(count);
	}
}

public class C_RoomExit : IPacket
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

public class S_RoomExit : IPacket
{
	public int Index;

	public ushort Protocol { get { return (ushort)PacketID.S_RoomExit; } }

	public void Deserialize(ArraySegment<byte> segment)
	{
		ushort count = 0;

		count += sizeof(ushort);
		count += sizeof(ushort);
		count += PacketUtility.ReadIntData(segment, count, out Index);
	}

	public ArraySegment<byte> Serialize()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		ushort count = 0;

		count += sizeof(ushort);
		count += PacketUtility.AppendUshortData(this.Protocol, segment, count);
		count += PacketUtility.AppendIntData(this.Index, segment, count);
		PacketUtility.AppendUshortData(count, segment, 0);
		return SendBufferHelper.Close(count);
	}
}

public class C_CreateRoom : IPacket
{
	public string roomName;

	public ushort Protocol { get { return (ushort)PacketID.C_CreateRoom; } }

	public void Deserialize(ArraySegment<byte> segment)
	{
		ushort count = 0;

		count += sizeof(ushort);
		count += sizeof(ushort);
		count += PacketUtility.ReadStringData(segment, count, out roomName);
	}

	public ArraySegment<byte> Serialize()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		ushort count = 0;

		count += sizeof(ushort);
		count += PacketUtility.AppendUshortData(this.Protocol, segment, count);
		count += PacketUtility.AppendStringData(this.roomName, segment, count);
		PacketUtility.AppendUshortData(count, segment, 0);
		return SendBufferHelper.Close(count);
	}
}

public class S_RoomList : IPacket
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

public class C_RoomList : IPacket
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

public class S_TestText : IPacket
{
	public string text;

	public ushort Protocol { get { return (ushort)PacketID.S_TestText; } }

	public void Deserialize(ArraySegment<byte> segment)
	{
		ushort count = 0;

		count += sizeof(ushort);
		count += sizeof(ushort);
		count += PacketUtility.ReadStringData(segment, count, out text);
	}

	public ArraySegment<byte> Serialize()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		ushort count = 0;

		count += sizeof(ushort);
		count += PacketUtility.AppendUshortData(this.Protocol, segment, count);
		count += PacketUtility.AppendStringData(this.text, segment, count);
		PacketUtility.AppendUshortData(count, segment, 0);
		return SendBufferHelper.Close(count);
	}
}

public class S_EnterRoomFirst : IPacket
{
	public int myIndex;
	public List<LocationInfoPacket> playerLocations;
	public List<PlayerInfoPacket> playerInfos;

	public ushort Protocol { get { return (ushort)PacketID.S_EnterRoomFirst; } }

	public void Deserialize(ArraySegment<byte> segment)
	{
		ushort count = 0;

		count += sizeof(ushort);
		count += sizeof(ushort);
		count += PacketUtility.ReadIntData(segment, count, out myIndex);
		count += PacketUtility.ReadListData(segment, count, out playerLocations);
		count += PacketUtility.ReadListData(segment, count, out playerInfos);
	}

	public ArraySegment<byte> Serialize()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		ushort count = 0;

		count += sizeof(ushort);
		count += PacketUtility.AppendUshortData(this.Protocol, segment, count);
		count += PacketUtility.AppendIntData(this.myIndex, segment, count);
		count += PacketUtility.AppendListData(this.playerLocations, segment, count);
		count += PacketUtility.AppendListData(this.playerInfos, segment, count);
		PacketUtility.AppendUshortData(count, segment, 0);
		return SendBufferHelper.Close(count);
	}
}

public class S_UpdateInfos : IPacket
{
	public List<PlayerInfoPacket> playerInfos;
	public List<SnapshotPacket> snapshots;
	public List<AttackInfoBr> attacks;

	public ushort Protocol { get { return (ushort)PacketID.S_UpdateInfos; } }

	public void Deserialize(ArraySegment<byte> segment)
	{
		ushort count = 0;

		count += sizeof(ushort);
		count += sizeof(ushort);
		count += PacketUtility.ReadListData(segment, count, out playerInfos);
		count += PacketUtility.ReadListData(segment, count, out snapshots);
		count += PacketUtility.ReadListData(segment, count, out attacks);
	}

	public ArraySegment<byte> Serialize()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		ushort count = 0;

		count += sizeof(ushort);
		count += PacketUtility.AppendUshortData(this.Protocol, segment, count);
		count += PacketUtility.AppendListData(this.playerInfos, segment, count);
		count += PacketUtility.AppendListData(this.snapshots, segment, count);
		count += PacketUtility.AppendListData(this.attacks, segment, count);
		PacketUtility.AppendUshortData(count, segment, 0);
		return SendBufferHelper.Close(count);
	}
}

public class C_UpdateLocation : IPacket
{
	public bool isAiming;
	public LocationInfoPacket location;

	public ushort Protocol { get { return (ushort)PacketID.C_UpdateLocation; } }

	public void Deserialize(ArraySegment<byte> segment)
	{
		ushort count = 0;

		count += sizeof(ushort);
		count += sizeof(ushort);
		count += PacketUtility.ReadBoolData(segment, count, out isAiming);
		count += PacketUtility.ReadDataPacketData(segment, count, out location);
	}

	public ArraySegment<byte> Serialize()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		ushort count = 0;

		count += sizeof(ushort);
		count += PacketUtility.AppendUshortData(this.Protocol, segment, count);
		count += PacketUtility.AppendBoolData(this.isAiming, segment, count);
		count += PacketUtility.AppendDataPacketData(this.location, segment, count);
		PacketUtility.AppendUshortData(count, segment, 0);
		return SendBufferHelper.Close(count);
	}
}

public class S_TeamInfos : IPacket
{
	public List<TeamInfoPacket> teamInfos;

	public ushort Protocol { get { return (ushort)PacketID.S_TeamInfos; } }

	public void Deserialize(ArraySegment<byte> segment)
	{
		ushort count = 0;

		count += sizeof(ushort);
		count += sizeof(ushort);
		count += PacketUtility.ReadListData(segment, count, out teamInfos);
	}

	public ArraySegment<byte> Serialize()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		ushort count = 0;

		count += sizeof(ushort);
		count += PacketUtility.AppendUshortData(this.Protocol, segment, count);
		count += PacketUtility.AppendListData(this.teamInfos, segment, count);
		PacketUtility.AppendUshortData(count, segment, 0);
		return SendBufferHelper.Close(count);
	}
}

public class S_UpdateLocations : IPacket
{
	public List<LocationInfoPacket> locations;

	public ushort Protocol { get { return (ushort)PacketID.S_UpdateLocations; } }

	public void Deserialize(ArraySegment<byte> segment)
	{
		ushort count = 0;

		count += sizeof(ushort);
		count += sizeof(ushort);
		count += PacketUtility.ReadListData(segment, count, out locations);
	}

	public ArraySegment<byte> Serialize()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		ushort count = 0;

		count += sizeof(ushort);
		count += PacketUtility.AppendUshortData(this.Protocol, segment, count);
		count += PacketUtility.AppendListData(this.locations, segment, count);
		PacketUtility.AppendUshortData(count, segment, 0);
		return SendBufferHelper.Close(count);
	}
}

public class C_ShootReq : IPacket
{
	public int hitPlayerIndex;
	public VectorPacket firePos;
	public VectorPacket direction;

	public ushort Protocol { get { return (ushort)PacketID.C_ShootReq; } }

	public void Deserialize(ArraySegment<byte> segment)
	{
		ushort count = 0;

		count += sizeof(ushort);
		count += sizeof(ushort);
		count += PacketUtility.ReadIntData(segment, count, out hitPlayerIndex);
		count += PacketUtility.ReadDataPacketData(segment, count, out firePos);
		count += PacketUtility.ReadDataPacketData(segment, count, out direction);
	}

	public ArraySegment<byte> Serialize()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		ushort count = 0;

		count += sizeof(ushort);
		count += PacketUtility.AppendUshortData(this.Protocol, segment, count);
		count += PacketUtility.AppendIntData(this.hitPlayerIndex, segment, count);
		count += PacketUtility.AppendDataPacketData(this.firePos, segment, count);
		count += PacketUtility.AppendDataPacketData(this.direction, segment, count);
		PacketUtility.AppendUshortData(count, segment, 0);
		return SendBufferHelper.Close(count);
	}
}

