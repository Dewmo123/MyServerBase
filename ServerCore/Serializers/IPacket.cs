using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerCore.Serializers
{
    public interface IPacket : IPacketSerializable
    {
        ushort Protocol { get; }
    }
    public interface IDataPacket : IPacketSerializable
    {
    }
}
