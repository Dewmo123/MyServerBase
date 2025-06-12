using Server.Objects;
using Server.Objects.Areas;
using Server.Utiles;
using System;

namespace Server.Rooms.States
{
    internal abstract class GamingState : SyncObjectsState
    {
        public GamingState(GameRoom room) : base(room)
        {
        }
        public override void Enter()
        {
            base.Enter();
            _room.OnDoorStatusChange += HandleDoorStatusChange;
        }
        public override void Exit()
        {
            base.Exit();
            _room.OnDoorStatusChange -= HandleDoorStatusChange;
        }
        private void HandleDoorStatusChange(DoorStatus targetStatus, Door door, Player player)
        {
            S_DoorStatus doorStatus = new();
            doorStatus.index = door.index;
            doorStatus.status = (ushort)door.GetNegate();//판별도 넣기
            door.Status = targetStatus;
            Console.WriteLine($"door: {doorStatus.status}, target:{targetStatus}");
            _room.Broadcast(doorStatus);
        }
    }
}