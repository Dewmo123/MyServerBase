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
class C_Chat : IPacket
{
    public string chat;

    public ushort Protocol { get { return (ushort)PacketID.C_Chat; } }

    public void Deserialize(ArraySegment<byte> segment)
    {
        ushort count = 0;

        ReadOnlySpan<byte> s = new ReadOnlySpan<byte>(segment.Array, segment.Offset, segment.Count);
        count += sizeof(ushort);
        count += sizeof(ushort);
        ushort chatLen = BitConverter.ToUInt16(s.Slice(count, s.Length - count));
        count += sizeof(ushort);
        this.chat = Encoding.Unicode.GetString(s.Slice(count, chatLen));
        count += chatLen;
    }

    public ArraySegment<byte> Serialize()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;
        bool success = true;

        Span<byte> s = new Span<byte>(segment.Array, segment.Offset, segment.Count);

        count += sizeof(ushort);
        success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), (ushort)PacketID.C_Chat);
        count += sizeof(ushort);
        ushort chatLen = (ushort)Encoding.Unicode.GetBytes(this.chat, 0, this.chat.Length, segment.Array, segment.Offset + count + sizeof(ushort));
        success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), chatLen);
        count += sizeof(ushort);
        count += chatLen;
        success &= BitConverter.TryWriteBytes(s, count);
        if (success == false)
            return null;
        return SendBufferHelper.Close(count);
    }
}

class S_Chat : IPacket
{
    public int playerId;
    public string chat;

    public ushort Protocol { get { return (ushort)PacketID.S_Chat; } }

    public void Deserialize(ArraySegment<byte> segment)
    {
        ushort count = 0;

        count += sizeof(ushort);
        count += sizeof(ushort);

        count += PacketUtility.ReadIntData(segment, count, out playerId);
        count += PacketUtility.ReadStringData(segment, count, out chat);
    }

    public ArraySegment<byte> Serialize()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort process = 0;

        process += sizeof(ushort);
        process += PacketUtility.AppendUShortData(Protocol, segment, process);
        process += PacketUtility.AppendIntData(playerId, segment, process);
        process += PacketUtility.AppendStringData(chat, segment, process);
        PacketUtility.AppendUShortData(process, segment, 0);

        return SendBufferHelper.Close(process);
    }
}

