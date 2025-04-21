using Server;
using Server.Rooms;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Text;

class PacketHandler
{
    private static RoomManager _roomManager = RoomManager.Instance;
    internal static void C_CreateRoomHandler(PacketSession session, IPacket packet)
    {
        var createRoom = packet as C_CreateRoom;
        var clientSession = session as ClientSession;
        int roomId = _roomManager.GenerateRoom(createRoom.roomName);
        EnterRoomProcess(roomId, clientSession);
    }

    internal static void C_RoomEnterHandler(PacketSession session, IPacket packet)
    {
        var enterPacket = packet as C_RoomEnter;
        var clientSession = session as ClientSession;
        EnterRoomProcess(enterPacket.roomId, clientSession);
    }

    private static void EnterRoomProcess(int roomId, ClientSession clientSession)
    {
        var room = _roomManager.GetRoomById(roomId);
        if (room == default)
            throw new NullReferenceException();
        clientSession.myInfo = new PlayerInfoPacket()
        {
            direction = new VectorPacket(),
            position = new VectorPacket(),
            index = clientSession.SessionId
        };
        _roomManager.EnterRoom(clientSession, roomId);
        room.SendAllPlayerInfosFirst(clientSession.SessionId);
        room.Push(() => room.Broadcast(new S_RoomEnter() { newPlayer = clientSession.myInfo }));
    }

    internal static void C_RoomExitHandler(PacketSession session, IPacket packet)
    {
        var clientSession = session as ClientSession;
        var room = clientSession.Room;
        room.Push(() => room.Leave(clientSession.SessionId));
        room.Push(() => room.Broadcast(new S_RoomExit() { Index = clientSession.SessionId }));
        Console.WriteLine($"Leave Room: {clientSession.SessionId}");
    }

    internal static void C_RoomListHandler(PacketSession session, IPacket packet)
    {
        var clientSession = session as ClientSession;
        var list = _roomManager.GetRoomInfos();
        S_RoomList roomList = new S_RoomList();
        roomList.roomInfos = list;
        clientSession.Send(roomList.Serialize());
    }
}
