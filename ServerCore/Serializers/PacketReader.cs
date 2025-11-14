using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace ServerCore.Serializers
{
    public struct PacketReader : IPacketSerializer
    {
        private ArraySegment<byte> buffer;
        private ushort offset;
        private static readonly Encoding encoding = Encoding.UTF8;
        public PacketReader(ArraySegment<byte> buffer, ushort offset = 0)
        {
            this.buffer = buffer;
            this.offset = offset;
        }
        public void SerializeObject<T>(ref T value) where T : IPacketSerializable
        {
            value.Serialize(ref this);
        }

        public void SerializeObject<T>(ref T[] values) where T : IPacketSerializable
        {
            ushort count = 0;
            Serialize(ref count);

            values = new T[count];
            for (int i = 0; i < count; i++)
                SerializeObject(ref values[i]);
        }

        public void SerializeObject<T>(ref List<T> values) where T : IPacketSerializable, new()
        {
            ushort count = 0;
            Serialize(ref count);
            values = new List<T>(count);
            for (int i = 0; i < count; i++)
            {
                T value = new T();
                SerializeObject(ref value);
                values.Add(value);
            }
        }
        public unsafe void Serialize<T>(ref T value) where T : unmanaged
        {
            ushort size = (ushort)sizeof(T);
            if (buffer.Count - offset < size)
                throw new Exception($"Not enough buffer: required {size}, available {buffer.Count - offset}");

            value = MemoryMarshal.Read<T>(new Span<byte>(buffer.Array, buffer.Offset + offset, size));
            offset += size;
        }

        public void Serialize<T>(ref T[] values) where T : unmanaged
        {
            ushort count= 0;
            Serialize(ref count);

            values = new T[count];
            for (int i = 0; i < count; i++)
                Serialize(ref values[i]);
        }

        public void Serialize<T>(ref List<T> values) where T : unmanaged
        {
            ushort count = 0;
            Serialize(ref count);
            values = new List<T>(count);
            for (int i = 0; i < count; i++)
            {
                T value = new T();
                Serialize(ref value);
                values.Add(value);
            }
        }

        public void Serialize(ref string value)
        {
            value ??= string.Empty;

            ushort size = 0;
            Serialize(ref size);
            if (buffer.Count - offset < size)
                throw new Exception($"Not enough buffer: required {size}, available {buffer.Count - offset}");

            value = encoding.GetString(buffer.Array, buffer.Offset + offset, size);
            offset += size;
        }

        public void Serialize(ref string[] values)
        {
            ushort count = 0;
            Serialize(ref count);

            values = new string[count];
            for (int i = 0; i < count; i++)
                Serialize(ref values[i]);
        }

        public void Serialize(ref List<string> values)
        {
            ushort count = 0;
            Serialize(ref count);

            values = new List<string>(count);
            for (int i = 0; i < count; i++)
            {
                string value = string.Empty;
                Serialize(ref value);
                values.Add(value);
            }
        }
    }
}
