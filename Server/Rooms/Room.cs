using Server.Objects;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Rooms
{
    internal abstract class Room : IJobQueue
    {
        protected Dictionary<int, ObjectBase> _objects = new();
        private int _objectIdGenerator = 0;
        protected RoomManager _roomManager;
        public Room(RoomManager manager, int roomId, string name)
        {
            _roomManager = manager;
            RoomId = roomId;
            RoomName = name;

        }
        protected Dictionary<int, ClientSession> _sessions = new Dictionary<int, ClientSession>();
        private JobQueue _jobQueue = new JobQueue();
        private List<ArraySegment<byte>> _pendingList = new List<ArraySegment<byte>>();
        public string RoomName { get; private set; }
        public int RoomId { get; private set; } = 0;
        public int MaxSessionCount { get; private set; } = 10;//임의
        public int SessionCount => _sessions.Count;
        public Dictionary<int, ClientSession> Sessions => _sessions;

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
        public ClientSession GetSession(int key)
        {
            return _sessions[key];
        }
        public void Enter(ClientSession session)
        {
            _sessions.Add(session.SessionId, session);
            Console.WriteLine(SessionCount);
            session.Room = this;
        }
        public void Leave(ClientSession session)
        {
            _sessions.Remove(session.SessionId);
            _objects.Remove(session.PlayerId);
            if (SessionCount == 0)
            {
                _roomManager.RemoveRoom(RoomId);
            }
            else
            {
                Broadcast(new S_RoomExit() { Index = session.PlayerId });
            }
        }
        public void AddObject(ObjectBase obj)
        {
            _objects.Add(++_objectIdGenerator, obj);
            //Console.WriteLine($"add:{_objectIdGenerator}");
            obj.index = _objectIdGenerator;
        }
        public T GetObject<T>(int id) where T : ObjectBase
        {
            return _objects.GetValueOrDefault(id) as T;
        }
        public abstract void UpdateRoom();
    }
}
