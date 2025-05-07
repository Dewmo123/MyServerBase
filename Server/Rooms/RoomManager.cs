﻿using ServerCore;
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
        public void UpdateRooms()
        {
            try
            {
                _rwLock.EnterReadLock();
                foreach (var room in _rooms.Values)
                    room.Push(() => room.UpdateRoom());
            }
            finally
            {
                _rwLock.ExitReadLock();
            }
        }
        public void FlushRooms()
        {
            try
            {
                _rwLock.EnterReadLock();
                foreach (var room in _rooms.Values)
                    room.Push(() => room.Flush());
            }
            finally
            {
                _rwLock.ExitReadLock();
            }
        }
        public void EnterRoom(ClientSession session, int roomId)
        {

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
        public GameRoom GetRoomById(int roomId)
        {
            try
            {
                _rwLock.EnterReadLock();
                return _rooms.GetValueOrDefault(roomId);
            }
            finally
            {
                _rwLock.ExitReadLock();
            }
        }
        public int GenerateRoom(string roomName)
        {
            try
            {
                _rwLock.EnterWriteLock();
                int id = ++_roomIdGenerator;
                Console.WriteLine($"Generate Room: {id}");
                GameRoom room = new(Instance,roomName, id);
                _rooms.Add(id, room);
                return id;
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
                        roomName = room.Value.RoomName,
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
