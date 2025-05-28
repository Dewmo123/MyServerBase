using Server;
using Server.Objects;
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
        if (clientSession.Room != null)
            return;
        EnterRoomProcess(enterPacket.roomId, clientSession);
        Console.WriteLine("EnterRoom");
    }

    private static void EnterRoomProcess(int roomId, ClientSession clientSession)
    {
        var room = _roomManager.GetRoomById(roomId) as GameRoom;
        if (room == default)
            throw new NullReferenceException();
        Player newPlayer = new Player();
        if (room.CanAddPlayer)
        {
            room.Push(() =>
            {
                room.AddObject(newPlayer);
                clientSession.PlayerId = newPlayer.index;
                room.Enter(clientSession);
                room.FirstEnterProcess(clientSession);
                LocationInfoPacket location = new() { index = newPlayer.index };
                room.Broadcast(new S_RoomEnter() { newPlayer = location });
            });
        }
    }

    internal static void C_RoomExitHandler(PacketSession session, IPacket packet)
    {
        var clientSession = session as ClientSession;
        var room = clientSession.Room;
        room.Push(() => room.Leave(clientSession));
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

    internal static void C_UpdateLocationHandler(PacketSession session, IPacket packet)
    {
        var clientSession = session as ClientSession;
        var playerPacket = packet as C_UpdateLocation;
        Player player = clientSession.Room.GetObject<Player>(clientSession.PlayerId);
        player.HandlePacket(playerPacket);
    }

    internal static void C_ShootReqHandler(PacketSession session, IPacket packet)
    {
        var clientSession = session as ClientSession;
        var shootReq = packet as C_ShootReq;
        if (clientSession.Room == null)
            return;
        var room = clientSession.Room as GameRoom;
        room.Push(() => room.Attack(clientSession, shootReq));
    }
}
