using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
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
	public static Stopwatch timer;
        static void Main(string[] args)
        {
            // DNS (Domain Name System)
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 3303);
	    timer = new();
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
            Timer flushTimer = new Timer(15);
		timer.Restart();
            flushTimer.Elapsed += UpdateLoop;
            flushTimer.Enabled = true;
            flushTimer.AutoReset = true;
        }

        private static void UpdateLoop(object sender, ElapsedEventArgs e)
        {
		//Console.WriteLine(timer.ElapsedMilliseconds);
            roomManager.UpdateRooms();
            roomManager.FlushRooms();
        }
    }
}

