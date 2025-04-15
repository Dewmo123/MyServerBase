using Server;
using Server.Rooms;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Text;

class PacketHandler
{
    internal static void C_RoomEnterHandler(PacketSession session, IPacket packet)
    {
        var enterPacket = packet as C_RoomEnter;
        var clientSession = session as ClientSession;
        RoomManager.Instance.EnterRoom(clientSession, enterPacket.roomId);
    }

    internal static void C_RoomListHandler(PacketSession session, IPacket packet)
    {
    }
}
