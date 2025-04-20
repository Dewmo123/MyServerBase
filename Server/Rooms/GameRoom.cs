using ServerCore;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Server.Rooms
{
    class GameRoom : IJobQueue
    {
        private RoomManager _roomManager;
        public int RoomId { get; private set; } = 0;
        public bool CanAddPlayer => SessionCount < MaxSessionCount;
        public GameRoom(RoomManager roomManager,int roomId)
        {
            RoomId = roomId;
            _roomManager = roomManager;
        }
        public int MaxSessionCount { get; private set; } = 15;//임의
        public int SessionCount => _sessions.Count;

        Dictionary<int,ClientSession> _sessions = new Dictionary<int, ClientSession>();
        JobQueue _jobQueue = new JobQueue();
        List<ArraySegment<byte>> _pendingList = new List<ArraySegment<byte>>();
        public void Push(Action job)
        {
            _jobQueue.Push(job);
        }

        public void Flush()
        {
            // N ^ 2
            //Console.WriteLine("Flush");
            foreach (ClientSession s in _sessions.Values)
                s.Send(_pendingList);

            //Console.WriteLine($"Flushed {_pendingList.Count} items");
            _pendingList.Clear();
        }

        public void Broadcast(IPacket packet)
        {
            _pendingList.Add(packet.Serialize());
        }
        public void Enter(ClientSession session)
        {
            _sessions.Add(session.SessionId,session);
            session.Room = this;
        }

        public void Leave(int sessionId)
        {
            _sessions.Remove(sessionId);
            if (SessionCount == 0)
            {
                _roomManager.RemoveRoom(RoomId);
            }
        }
        public void SendAllPlayerInfosFirst(int sessionId)
        {
            S_EnterRoomFirst players = new();
            players.playerInfos = new List<PlayerInfoPacket>();
            Push(() =>
            {
                Console.WriteLine("SendAllPlayer");
                players.myIndex = sessionId;
                foreach (var player in _sessions)
                {
                    players.playerInfos.Add(new PlayerInfoPacket()
                    {
                        index = player.Key,
                        direction = player.Value.myInfo.direction,
                        position = player.Value.myInfo.position
                    });
                }
                _sessions[sessionId].Send(players.Serialize());
            });
        }
        public void UpdateRoom()
        {
            S_UpdateInfos updates = new();
            updates.playerInfos = new List<PlayerInfoPacket>();
            foreach(var player in _sessions)
            {
                updates.playerInfos.Add(new PlayerInfoPacket()
                {
                    index = player.Key,
                    direction = player.Value.myInfo.direction,
                    position = player.Value.myInfo.position
                });
            }
            Broadcast(updates);
        }
    }
}
