using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Server.Rooms;
using ServerCore;

namespace Server
{
    class Program
    {
        static Listener _listener = new Listener();
        public static RoomManager roomManager = RoomManager.Instance;

        static void Main(string[] args)
        {
            // DNS (Domain Name System)
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 7777);

            _listener.Init(endPoint, () => { return SessionManager.Instance.Generate(); });
            Console.WriteLine("Listening...");
            roomManager.GenerateRoom("ASDASD");
            InitFlushTimer();
            //InitUpdateTimer();
            while (true) { }
            //FlushRoom();
        }
        //private static void InitUpdateTimer()
        //{
        //    Timer updateTimer = new Timer(30);
        //    updateTimer.Enabled = true;
        //    updateTimer.AutoReset = true;
        //}
        private static void InitFlushTimer()
        {
            Timer flushTimer = new Timer(10);
            flushTimer.Elapsed += UpdateLoop;
            flushTimer.Enabled = true;
            flushTimer.AutoReset = true;
        }

        private static void UpdateLoop(object sender, ElapsedEventArgs e)
        {
            roomManager.UpdateRooms();
            roomManager.FlushRooms();
            //Console.WriteLine($"Update: {DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}");
        }
    }
}
