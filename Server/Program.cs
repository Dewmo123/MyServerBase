using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Server.Rooms;
using ServerCore;

namespace Server
{
	class Program
	{
		static Listener _listener = new Listener();
		public static RoomManager roomManager = RoomManager.Instance;

		static void FlushRoom()
		{
            roomManager.FlushRooms();
			JobTimer.Instance.Push(FlushRoom, 250);
		}

		static void Main(string[] args)
		{
			// DNS (Domain Name System)
			IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 7777);

			_listener.Init(endPoint, () => { return SessionManager.Instance.Generate(); });
			Console.WriteLine("Listening...");
			roomManager.GenerateRoom("ASDASD");
			//FlushRoom();
			JobTimer.Instance.Push(FlushRoom);

			while (true)
			{
				JobTimer.Instance.Flush();
			}
		}
	}
}
