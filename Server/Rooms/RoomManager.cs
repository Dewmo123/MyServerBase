using ServerCore;
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
            if(_rooms.TryGetValue(roomId, out GameRoom room))
                return room.Enter(session);
            return false;
        }
    }
}
