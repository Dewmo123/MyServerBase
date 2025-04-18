using ServerCore;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Server.Rooms
{
    class RoomManager : Singleton<RoomManager>
    {

        private Dictionary<int, GameRoom> _rooms = new Dictionary<int, GameRoom>();
        private ReaderWriterLockSlim _rwLock = new ReaderWriterLockSlim();
        private int _roomIdGenerator = 0;
        public void FlushRooms()
        {
            try
            {
                _rwLock.EnterReadLock();
                Console.WriteLine(_rooms.Count);
                foreach (var room in _rooms.Values)
                {
                    room.Push(() => room.Broadcast(new S_TestText() { text = $"Room Id: {room.RoomId}" }));
                    room.Push(() => room.Flush());
                }
            }
            finally
            {
                _rwLock.ExitReadLock();
            }
        }
        public bool EnterRoom(ClientSession session, int roomId)
        {
            try
            {
                _rwLock.EnterReadLock();
                if (_rooms.TryGetValue(roomId, out GameRoom room))
                    if (room.CanAddPlayer)
                    {
                        room.Push(() => room.Enter(session));
                        return true;
                    }
                return false;
            }
            finally
            {
                _rwLock.ExitReadLock();
            }
        }
        public void RemoveRoom(int roomId)
        {
            try
            {
                _rwLock.EnterWriteLock();
                _rooms.Remove(roomId);
            }
            finally
            {
                _rwLock.ExitWriteLock();
            }
        }
        public bool GenerateRoom(ClientSession session)
        {
            try
            {
                _rwLock.EnterWriteLock();
                int id = ++_roomIdGenerator;
                Console.WriteLine($"Generate Room: {id}");
                GameRoom room = new(Instance, id);
                _rooms.Add(id, room);
                _rwLock.ExitWriteLock();
                return EnterRoom(session, id);
            }
            finally
            {
                if (_rwLock.IsWriteLockHeld)
                    _rwLock.ExitWriteLock();
            }
        }
        public List<RoomInfoPacket> GetRoomInfos()
        {
            try
            {
                _rwLock.EnterReadLock();
                List<RoomInfoPacket> list = new List<RoomInfoPacket>();
                foreach (var room in _rooms)
                {
                    list.Add(new RoomInfoPacket()
                    {
                        roomId = room.Key,
                        maxCount = room.Value.MaxSessionCount,
                        currentCount = room.Value.SessionCount
                    });
                }
                return list;
            }
            finally
            {
                _rwLock.ExitReadLock();
            }
        }
    }
}
