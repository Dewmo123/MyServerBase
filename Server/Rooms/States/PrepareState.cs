using Server.Objects;
using ServerCore;
using System;
using System.Collections.Generic;

namespace Server.Rooms.States
{
    internal class PrepareState : GameRoomState
    {
        #region StartPos Settings
        private static readonly VectorPacket[] attackStartPos =
        {
            new VectorPacket() { x=10,y=-5.5f,z=-10},
            new VectorPacket() { x=5,y=-5.5f,z=-10},
            new VectorPacket() { x=0,y=-5.5f,z=-10},
            new VectorPacket() { x=-5,y=-5.5f,z=-10},
            new VectorPacket() { x=-10,y=-5.5f,z=-10},
        };
        private static readonly VectorPacket[] defenseStartPos =
        {
            new VectorPacket() { x=10,y=-5.5f,z=15},
            new VectorPacket() { x=5,y=-5.5f,z=15},
            new VectorPacket() { x=0,y=-5.5f,z=15},
            new VectorPacket() { x=-5,y=-5.5f,z=15},
            new VectorPacket() { x=-10,y=-5.5f,z=15},
        };
        private static Dictionary<Team, VectorPacket[]> startPos = new()
        {
            {Team.Attack,attackStartPos },
            {Team.Defense,defenseStartPos }
        };
        private static ushort[] randomTable = { 0, 1, 2, 3, 4 };
        #endregion

        private S_UpdateInfos updates = new();
        private Random _rand = new Random();
        public PrepareState(GameRoom room) : base(room)
        {
            updates.playerInfos = new List<PlayerInfoPacket>(15);
            updates.snapshots = new List<SnapshotPacket>(15);
            updates.attacks = new List<AttackInfoBr>(15);
        }
        public override void Enter()
        {
            base.Enter();
            Console.WriteLine("pREPARE");
            updates.playerInfos.Clear();
            updates.snapshots.Clear();
            updates.attacks.Clear();
            SetStartPosition();
        }

        private void SetStartPosition()
        {
            for (int i = 0; i < 10; i++)
            {
                int change = _rand.Next(0, randomTable.Length);
                (randomTable[0], randomTable[change]) = (randomTable[change], randomTable[0]);
            }
            int index = 0;
            S_UpdateLocations updateLocations = new();
            updateLocations.locations = new List<LocationInfoPacket>();
            foreach (var item in _room.Sessions)
            {
                Player player = _room.GetObject<Player>(item.Value.PlayerId); 
                updateLocations.locations.Add(new LocationInfoPacket()
                {
                    animHash = 0,
                    index = item.Key,
                    gunRotation = new QuaternionPacket(),
                    position = startPos[player.team][index % 5],
                    rotation = new()
                });
                index++;
            }
            _room.Broadcast(updateLocations);
        }

        public override void Update()
        {
            base.Update();
            List<AttackInfoBr> attacks = _room.GetAttacks();
            foreach (var session in _room.Sessions)
            {
                Player player = _room.GetObject<Player>(session.Value.PlayerId);
                updates.snapshots.Add(new SnapshotPacket()
                {
                    index = player.index,
                    position = player.position,
                    rotation = player.rotation,
                    animHash = player.animHash,
                    gunRotation = player.gunRotation,
                    timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
                });
                updates.playerInfos.Add(new PlayerInfoPacket()
                {
                    Health = player.Health,
                    index = player.index,
                    isAiming = player.isAiming
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
