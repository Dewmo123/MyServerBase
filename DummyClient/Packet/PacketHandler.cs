using DummyClient;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Text;

class PacketHandler
{
    internal static void S_RoomEnterHandler(PacketSession session, IPacket packet)
    {
        Console.WriteLine("RoomEnter");
    }

    internal static void S_RoomListHandler(PacketSession session, IPacket packet)
    {
        var listPacket = packet as S_RoomList;
        foreach(var item in listPacket.roomInfos)
        {
            Console.WriteLine($"{item.roomId}: {item.currentCount} / {item.maxCount}");
        }
    }

    internal static void S_TestTextHandler(PacketSession session, IPacket packet)
    {
        var test = packet as S_TestText;
        Console.WriteLine(test.text);
    }
}
