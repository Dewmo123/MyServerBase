using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace ServerCore.Serializers
{
    public struct PacketWriter : IPacketSerializer
    {
        private ArraySegment<byte> buffer;
        public ushort Offset { get; private set; }
        private static readonly Encoding encoding = Encoding.UTF8;
        public PacketWriter(ArraySegment<byte> buffer, ushort offset = 0)
        {
            this.buffer = buffer;
            this.Offset = offset;
        }
        public void SerializeObject<T>(ref T value) where T : IPacketSerializable
        {
            value.Serialize(ref this);
        }

        public void SerializeObject<T>(ref T[] values) where T : IPacketSerializable
        {
            ushort count = (ushort)values.Length;
            Serialize(ref count);
            for (int i = 0; i < count; i++)
                SerializeObject(ref values[i]);
        }

        public void SerializeObject<T>(ref List<T> values) where T : IPacketSerializable, new()
        {
            ushort count = (ushort)values.Count;
            Serialize(ref count);
            for (int i = 0; i < count; i++)
            {
                IPacketSerializable value = values[i];
                SerializeObject(ref value);
            }
        }
        public unsafe void Serialize<T>(ref T value) where T : unmanaged
        {
            ushort size = (ushort)sizeof(T);

            if (buffer.Count - Offset < size)
                throw new Exception($"Not enuogh buffer: required : {size}, available {buffer.Count - Offset}");

            MemoryMarshal.Write(new Span<byte>(buffer.Array, buffer.Offset + Offset, size), ref value);
            Offset += size;
        }

        public void Serialize<T>(ref T[] values) where T : unmanaged
        {
            ushort count = (ushort)values.Length;
            Serialize(ref count);

            for (int i = 0; i < count; i++)
                Serialize(ref values[i]);
        }

        public void Serialize<T>(ref List<T> values) where T : unmanaged
        {
            ushort count = (ushort)values.Count;
            Serialize(ref count);

            for (int i = 0; i < count; i++)
            {
                T value = values[i];
                Serialize(ref value);
            }
        }

        public void Serialize(ref string value)
        {
            value ??= string.Empty;
            ushort size = (ushort)encoding.GetByteCount(value);
            Serialize(ref size);
            if (buffer.Count - Offset < size)
                throw new Exception($"Not enuogh buffer: required : {size}, available {buffer.Count - Offset}");

            encoding.GetBytes(value, new Span<byte>(buffer.Array, buffer.Offset + Offset, size));
            Offset += size;
        }

        public void Serialize(ref string[] values)
        {
            ushort count = (ushort)values.Length;
            Serialize(ref count);

            for (int i = 0; i < count; i++)
                Serialize(ref values[i]);
        }

        public void Serialize(ref List<string> values)
        {
            ushort count = (ushort)values.Count;
            Serialize(ref count);

            for (int i = 0; i < count; i++)
            {
                string value = values[i];
                Serialize(ref value);
            }
        }
    }
}
