using System;
using System.Collections.Generic;

namespace ServerCore.Serializers
{
    public interface IPacketSerializer
    {
        void SerializeObject<T>(ref T value) where T : IPacketSerializable;
        void SerializeObject<T>(ref T[] values) where T : IPacketSerializable;
        void SerializeObject<T>(ref List<T> values) where T : IPacketSerializable, new();

        void Serialize<T>(ref T value) where T : unmanaged;
        void Serialize<T>(ref T[] values) where T : unmanaged;
        void Serialize<T>(ref List<T> values) where T : unmanaged;

        void Serialize(ref string value);
        void Serialize(ref string[] values);
        void Serialize(ref List<string> values);
    }
}
