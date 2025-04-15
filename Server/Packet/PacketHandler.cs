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
        var clientSession = session as ClientSession;
        Console.WriteLine("ASD");
        _roomManager.GenerateRoom(clientSession);
    }

    internal static void C_RoomEnterHandler(PacketSession session, IPacket packet)
    {
        var enterPacket = packet as C_RoomEnter;
        var clientSession = session as ClientSession;
        _roomManager.EnterRoom(clientSession, enterPacket.roomId);
    }

    internal static void C_RoomExitHandler(PacketSession session, IPacket packet)
    {
        var clientSession = session as ClientSession;
        clientSession.Room.Leave(clientSession);
    }

    internal static void C_RoomListHandler(PacketSession session, IPacket packet)
    {
        var clientSession = session as ClientSession;
        var list = _roomManager.GetRoomInfos();
        C_RoomList roomList = new C_RoomList();
        roomList.roomInfos = list;
        clientSession.Send(roomList.Serialize());
    }
}
