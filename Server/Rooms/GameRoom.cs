using Server.Objects;
using Server.Rooms.States;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Xml.Linq;

namespace Server.Rooms
{
    //state pattern으로 룸을 관리하면?
    //공통: 
    //플레이어 대기 상태 - Enter: 딱히 할게 없어보임. Update: 뭐 플레이어 팀정하기? 아바타 동기화, Exit: 딱히 할게없어보임
    //라운드 준비 상태 - Enter: 팀별로 시작지점 보내기 보더 세우기, Update: 라운드 시작까지 남은시간 계산, 위치 동기화, Exit: 딱히 없지않나
    //게임 중 상태 - Enter: 보더 없애기, Update: 공격 패킷, 위치 동기화, 플레이어 카운트 세기, Exit: 없어보임
    public enum Team
    {
        Defense,
        Attack
    }
    internal class GameRoom : Room
    {
        public bool CanAddPlayer => SessionCount < MaxSessionCount && _stateMachine.CurrentState == "Lobby";
        private RoomStateMachine _stateMachine;
        private List<C_UpdateLocation> _playerInfos = new(15);
        private List<AttackInfoBr> _attacks = new(15);
        public GameRoom(RoomManager roomManager, string name, int roomId) : base(roomManager, roomId, name)
        {
            _stateMachine = new RoomStateMachine(this);
            ChangeState("Lobby");
        }
        #region Network

        #endregion

        public void FirstEnterProcess(ClientSession session)
        {
            S_EnterRoomFirst players = new();
            players.playerLocations = new List<LocationInfoPacket>();
            players.playerInfos = new();
            Console.WriteLine("SendAllPlayer");
            players.myIndex = session.PlayerId;
            foreach (var item in _sessions.Values)
            {
                var player = GetObject<Player>(item.PlayerId);
                LocationInfoPacket location = new LocationInfoPacket()
                {
                    animHash = player.animHash,
                    gunRotation = player.gunRotation,
                    index = player.index,
                    position = player.position,
                    rotation = player.rotation
                };
                players.playerLocations.Add(location);
                players.playerInfos.Add(new PlayerInfoPacket()
                {
                    Health = player.Health,
                    index = player.index,
                    isAiming = player.isAiming
                });
            }
            session.Send(players.Serialize());
        }

        public void Attack(ClientSession session, C_ShootReq req)
        {
            //로직 처리는 나중에
            _attacks.Add(new AttackInfoBr()
            {
                attackerIndex = session.PlayerId,
                direction = req.direction,
                firePos = req.firePos,
                hitPlayerIndex = req.hitPlayerIndex
            });
        }
        public List<AttackInfoBr> GetAttacks()
        {
            List<AttackInfoBr> attacks = new List<AttackInfoBr>(_attacks);
            _attacks.Clear();
            return attacks;
        }
        public List<int> GetSessionKeys()
            => _sessions.Keys.ToList();
        #region StateMachine
        public void ChangeState(string name)
            => _stateMachine.ChangeState(name);
        public override void UpdateRoom()
            => _stateMachine.UpdateRoom();
        #endregion
    }
}
