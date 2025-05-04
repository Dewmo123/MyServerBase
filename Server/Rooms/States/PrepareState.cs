using ServerCore;
using System;
using System.Collections.Generic;

namespace Server.Rooms.States
{
    internal class PrepareState : GameRoomState
    {
        public PrepareState(GameRoom room) : base(room)
        {
        }
        public override void Update()
        {
            base.Update();
            S_UpdateInfos updates = new();
            updates.playerInfos = new List<PlayerInfoPacket>();
            updates.snapshots = new List<SnapshotPacket>();
            var infos = _room.GetPlayerInfos();
            foreach (var info in infos)
            {
                updates.playerInfos.Add(info);
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
            _room.Broadcast(updates);
        }
    }
}
