using ServerCore;
using System;
using System.Collections.Generic;

namespace Server.Rooms.States
{
    internal class PrepareState : GameRoomState
    {
        private S_UpdateInfos updates = new();
        public PrepareState(GameRoom room) : base(room)
        {
            updates.playerInfos = new List<LocationInfoPacket>(15);
            updates.snapshots = new List<SnapshotPacket>(15);
            updates.attacks = new List<AttackInfoBr>(15);
        }
        public override void Enter()
        {
            base.Enter();
            updates.playerInfos.Clear();
            updates.snapshots.Clear();
            updates.attacks.Clear();
            var keys = _room.GetSessionKeys();
            foreach (int key in keys)
            {
                updates.playerInfos.Add(new LocationInfoPacket()
                {
                    animHash = 0,
                    index = key,
                    isAiming = false,
                    mouse = new VectorPacket(),
                    position = new VectorPacket(),
                    rotation = new QuaternionPacket()
                });
            }
        }
        public override void Update()
        {
            base.Update();
            var locations = _room.GetPlayerInfos();
            var attacks = _room.GetAttacks();
            foreach (var info in locations)
            {
                updates.snapshots.Add(new SnapshotPacket()
                {
                    index = info.index,
                    position = info.position,
                    rotation = info.rotation,
                    animHash = info.animHash,
                    timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
                });
                //Console.WriteLine(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
            }
            updates.attacks = attacks;
            _room.Broadcast(updates);
            updates.playerInfos.Clear();
            updates.snapshots.Clear();
        }
    }
}
