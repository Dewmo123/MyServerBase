using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using ServerCore;

namespace DummyClient
{
	class ServerSession : PacketSession
	{
		public override void OnConnected(EndPoint endPoint)
		{
			Console.WriteLine($"OnConnected : {endPoint}");
			C_CreateRoom pak = new C_CreateRoom();
            Console.WriteLine("asd");
            Send(pak.Serialize());
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
