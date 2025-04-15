using ServerCore;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Server.Rooms
{
    class RoomManager
    {
        private static RoomManager instance;
        public static RoomManager Instance => instance ??= new RoomManager();

        private Dictionary<int, GameRoom> _rooms = new Dictionary<int, GameRoom>();
        private int _roomIdGenerator = 0;
        public void FlushRooms()
        {
            foreach (var room in _rooms.Values)
                room.Push(() => room.Flush());
        }
        public bool EnterRoom(ClientSession session, int roomId)
        {
            if (_rooms.TryGetValue(roomId, out GameRoom room))
                return room.Enter(session);
            return false;
        }
        public void RemoveRoom(int roomId)
        {
            _rooms.Remove(roomId);
        }
        public bool GenerateRoom(ClientSession session)
        {
            int id = ++_roomIdGenerator;
            Console.WriteLine($"Generate Room: {id}");
            GameRoom room = new(Instance, id);
            _rooms.Add(id, room);
            return EnterRoom(session, id);
        }
        public List<RoomInfoPacket> GetRoomInfos()
        {
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
    }
}
