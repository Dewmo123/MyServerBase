using Server.Objects;
using Server.Utiles;
using System;
using System.Collections.Generic;

namespace Server.Rooms.States
{
    abstract class SyncObjectsState : GameRoomState
    {
        private S_UpdateInfos _updates = new();
        public SyncObjectsState(GameRoom room) : base(room)
        {
            _updates.playerInfos = new List<PlayerInfoPacket>(15);
            _updates.snapshots = new List<SnapshotPacket>(15);
            _updates.attacks = new List<AttackInfoBr>(15);
        }
        public override void Enter()
        {
            base.Enter();
            ResetPacket();
            _room.OnAttack += HandleAttack;
        }

        private void HandleAttack(ClientSession session, C_ShootReq req)
        {
            ObjectBase hitObj = _room.GetObject<ObjectBase>(req.hitObjIndex);
            Player attacker = _room.GetObject<Player>(session.PlayerId);
            if (attacker.IsDead)
                return;
            if (hitObj == null)
            {
                _updates.attacks.Add(new AttackInfoBr()
                {
                    attackerIndex = session.PlayerId,
                    direction = req.direction,
                    firePos = req.firePos,
                    isDead = false,
                    hitObjIndex = req.hitObjIndex,
                    objectType = 0
                });
            }
            if (hitObj is IHittable hittable)
            {
                if (hittable.IsDead)
                    return;
                if (_room.CurrentState != RoomState.Between)
                    hittable.Hit();
                _updates.attacks.Add(new AttackInfoBr()
                {
                    attackerIndex = session.PlayerId,
                    direction = req.direction,
                    firePos = req.firePos,
                    isDead = hittable.IsDead,
                    hitObjIndex = req.hitObjIndex,
                    objectType = (ushort)hitObj.ObjectType
                });

            }

        }
        public override void Exit()
        {
            base.Exit();
            if (_updates.attacks.Count > 0 || _updates.playerInfos.Count > 0 || _updates.snapshots.Count > 0)
            {
                _room.Broadcast(_updates);
                ResetPacket();
            }
            _room.OnAttack -= HandleAttack;
        }

        private void ResetPacket()
        {
            _updates.playerInfos.Clear();
            _updates.snapshots.Clear();
            _updates.attacks.Clear();
        }

        public override void Update()
        {
            base.Update();
            foreach (var session in _room.Sessions)
            {
                Player player = _room.GetObject<Player>(session.Value.PlayerId);
                // Console.WriteLine(player.index);
                _updates.snapshots.Add(new SnapshotPacket()
                {
                    index = player.index,
                    position = player.position.ToPacket(),
                    rotation = player.rotation.ToPacket(),
                    animHash = player.animHash,
                    gunRotation = player.gunRotation.ToPacket(),
                    timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
                });
                _updates.playerInfos.Add(new PlayerInfoPacket()
                {
                    Health = player.Health,
                    index = player.index,
                    isAiming = player.isAiming
                });
                //Console.WriteLine(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
            }
            _room.Broadcast(_updates);
            ResetPacket();
        }
    }
}
