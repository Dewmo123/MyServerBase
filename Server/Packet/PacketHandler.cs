using Server;
using Server.Rooms;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

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
        Console.WriteLine("EnterRoom");
    }

    private static void EnterRoomProcess(int roomId, ClientSession clientSession)
    {
        var room = _roomManager.GetRoomById(roomId);
        if (room == default)
            throw new NullReferenceException();
        clientSession.myInfo = new PlayerInfoPacket()
        {
            isAiming = false,
            position = new VectorPacket(),
            rotation = new QuaternionPacket(),
            mouse = new VectorPacket(),
            animHash = 0,
            index = clientSession.SessionId
        };
        if (_roomManager.EnterRoom(clientSession, roomId))
        {
            room.FirstEnterProcess(clientSession.SessionId);
            Thread.Sleep(100);
            room.Push(() =>
            {
                room.Broadcast(new S_RoomEnter() { newPlayer = clientSession.myInfo });
                Console.WriteLine("Broadcast");
            });
        }
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

    internal static void C_UpdateInfoHandler(PacketSession session, IPacket packet)
    {
        var clientSession = session as ClientSession;
        var playerPacket = packet as C_UpdateInfo;
        clientSession.myInfo = playerPacket.playerInfo;
    }
}
