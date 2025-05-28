using Server.Rooms;

namespace Server.Objects
{
    internal class Player : ObjectBase
    {
        public bool isAiming;
        public Team team;
        public QuaternionPacket gunRotation;
        public void HandlePacket(C_UpdateLocation packet)
        {
            isAiming = packet.isAiming;
            position = packet.location.position;
            rotation = packet.location.rotation;
            gunRotation = packet.location.gunRotation;
            animHash = packet.location.animHash;
        }
    }
}
