namespace Server.Objects
{
    internal class ObjectBase
    {
        public int index { get; set; }
        public int animHash;
        public int Health { get; set; }
        public VectorPacket position;
        public QuaternionPacket rotation;
    }
}
