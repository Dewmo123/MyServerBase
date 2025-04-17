using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using ServerCore;
using System.Threading;

namespace DummyClient
{
    class ServerSession : PacketSession
    {
        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnConnected : {endPoint}");
            C_CreateRoom pak = new C_CreateRoom();
            Send(pak.Serialize());
            C_RoomList pck = new();
            Console.WriteLine(pck.Protocol);
            var a = pck.Serialize();
            Console.WriteLine("asd: " + a.Array[6]);
            Send(a);
        }

        public override void OnDisconnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnDisconnected : {endPoint}");
        }

        public override void OnRecvPacket(ArraySegment<byte> buffer)
        {
            PacketManager.Instance.OnRecvPacket(this, buffer);
        }

        public override void OnSend(int numOfBytes)
        {
            Console.WriteLine("SEND");
            //Console.WriteLine($"Transferred bytes: {numOfBytes}");
        }
    }
}
