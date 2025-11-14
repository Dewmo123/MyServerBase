namespace ServerCore.Serializers
{
    public interface IPacketSerializable
    {
        void Serialize<T>(ref T serializer) where T : struct,IPacketSerializer;
    }
}
