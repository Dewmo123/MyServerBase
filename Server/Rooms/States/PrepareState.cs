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
            updates.playerInfos = new List<LocationInfoPacket>();
            updates.snapshots = new List<SnapshotPacket>();
        }
        public override void Enter()
        {
            base.Enter();
            updates.playerInfos.Clear();
            updates.snapshots.Clear();
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
            var infos = _room.GetPlayerInfos();
            foreach (var info in infos)
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
            Console.WriteLine(updates.playerInfos.Count);
            _room.Broadcast(updates);
            updates.playerInfos.Clear();
            updates.snapshots.Clear();
        }
    }
}
