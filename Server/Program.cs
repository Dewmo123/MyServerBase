using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Server.Rooms;
using ServerCore;
using Timer = System.Timers.Timer;


namespace Server
{
    class Program
    {
        static Listener _listener = new Listener();
        public static RoomManager roomManager = RoomManager.Instance;

        static void Main(string[] args)
        {
            // DNS (Domain Name System)
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 3303);

            _listener.Init(endPoint, () => { return SessionManager.Instance.Generate(); });
            Console.WriteLine("Listening...");
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
            Timer flushTimer = new Timer(50);

            flushTimer.Elapsed += UpdateLoop;
            flushTimer.Enabled = true;
            flushTimer.AutoReset = true;
            Timer syncTimer = new Timer(3000);
            syncTimer.Elapsed += SyncTime;
            syncTimer.Enabled = true;
            syncTimer.AutoReset = true;
        }
        static S_BroadcastTime time = new();
        private static void SyncTime(object sender, ElapsedEventArgs e)
        {
            time.time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            SessionManager.Instance.BroadcastAll(time);
        }

        private static void UpdateLoop(object sender, ElapsedEventArgs e)
        {
            roomManager.UpdateRooms();
            roomManager.FlushRooms();
        }
    }
}

