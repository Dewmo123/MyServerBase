using Server.Rooms.States;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace Server.Rooms
{
    //state pattern으로 룸을 관리하면?
    //공통: 
    //플레이어 대기 상태 - Enter: 딱히 할게 없어보임. Update: 뭐 플레이어 팀정하기? 아바타 동기화, Exit: 딱히 할게없어보임
    //라운드 준비 상태 - Enter: 팀별로 시작지점 보내기, Update: 라운드 시작까지 남은시간 계산, 위치 동기화, Exit: 딱히 없지않나
    //게임 중 상태 - Enter: 없어보임, Update: 공격 패킷, 위치 동기화, 플레이어 카운트 세기, Exit: 없어보임
    class GameRoom : IJobQueue
    {
        private RoomManager _roomManager;
        public int RoomId { get; private set; } = 0;
        public bool CanAddPlayer => SessionCount < MaxSessionCount;
        public string RoomName { get; private set; }
        private RoomStateMachine _stateMachine;
        public GameRoom(RoomManager roomManager, string name, int roomId)
        {
            RoomId = roomId;
            _roomManager = roomManager;
            RoomName = name;
            _stateMachine = new RoomStateMachine(this);
            _stateMachine.ChangeState("Lobby");
        }
        #region Network
        public int MaxSessionCount { get; private set; } = 15;//임의
        public int SessionCount => _sessions.Count;

        private Dictionary<int, ClientSession> _sessions = new Dictionary<int, ClientSession>();
        private JobQueue _jobQueue = new JobQueue();
        private List<ArraySegment<byte>> _pendingList = new List<ArraySegment<byte>>();
        public void Push(Action job)
        {
            _jobQueue.Push(job);
        }

        public void Flush()
        {
            // N ^ 2
            if (_pendingList.Count == 0)
                return;
            foreach (ClientSession s in _sessions.Values)
                s.Send(_pendingList);

            //Console.WriteLine($"Flushed {_pendingList.Count} items");
            _pendingList.Clear();
        }

        public void Broadcast(IPacket packet)
        {
            _pendingList.Add(packet.Serialize());
        }
        #endregion
        public void Enter(ClientSession session)
        {
            _sessions.Add(session.SessionId, session);
            session.Room = this;
        }
        public void Leave(int sessionId)
        {
            _sessions.Remove(sessionId);
            if (SessionCount == 0)
            {
                _roomManager.RemoveRoom(RoomId);
            }
            else
            {
                Broadcast(new S_RoomExit() { Index = sessionId });
            }
        }
        public void FirstEnterProcess(int sessionId)
        {
            S_EnterRoomFirst players = new();
            players.playerInfos = new List<PlayerInfoPacket>();
            Console.WriteLine("SendAllPlayer");
            players.myIndex = sessionId;
            foreach (var player in _sessions)
            {
                players.playerInfos.Add(player.Value.myInfo);
            }
            _sessions[sessionId].Send(players.Serialize());
        }
        private List<PlayerInfoPacket> _playerInfos = new();

        public List<PlayerInfoPacket> GetPlayerInfos()
        {
            _playerInfos.Clear();
            foreach (var session in _sessions.Values)
                _playerInfos.Add(session.myInfo);
            return _playerInfos;
        }

        public void ChangeState(string name)
            => _stateMachine.ChangeState(name);
        public void UpdateRoom()
            => _stateMachine.UpdateRoom();
    }
}
