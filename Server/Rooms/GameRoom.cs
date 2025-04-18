using ServerCore;
using System;
using System.Collections.Generic;
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

        List<ClientSession> _sessions = new List<ClientSession>();
        JobQueue _jobQueue = new JobQueue();
        List<ArraySegment<byte>> _pendingList = new List<ArraySegment<byte>>();
        public void Push(Action job)
        {
            _jobQueue.Push(job);
        }

        public void Flush()
        {
            // N ^ 2
            Console.WriteLine("Flush");
            foreach (ClientSession s in _sessions)
                s.Send(_pendingList);

            Console.WriteLine($"Flushed {_pendingList.Count} items");
            _pendingList.Clear();
        }

        public void Broadcast(IPacket packet)
        {
            _pendingList.Add(packet.Serialize());
        }

        public void Enter(ClientSession session)
        {
            _sessions.Add(session);
            session.Room = this;
            S_RoomEnter enterPacket = new();
            session.Send(enterPacket.Serialize());
        }

        public void Leave(ClientSession session)
        {
            _sessions.Remove(session);
            if (SessionCount == 0)
            {
                _roomManager.RemoveRoom(RoomId);
            }
        }
    }
}
