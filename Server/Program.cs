using Server.Rooms;
using Server.Utiles;
using ServerCore;
using System;
using System.Diagnostics;
using System.Net;


namespace Server
{
    class Program
    {
        static Listener _listener = new Listener();
        public static RoomManager roomManager = RoomManager.Instance;
        public static Stopwatch timer;
        private static long _frameCnt, _prevTick;
        private static float _frameTime;
        static void Main(string[] args)
        {
            // DNS (Domain Name System)
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 3303);
            timer = new();
            _listener.Init(endPoint, () => { return SessionManager.Instance.Generate(); });
            Console.WriteLine("Listening..."); 
            timer.Restart();
            while (true)
            {
                long currentTick = timer.ElapsedTicks;
                Time.deltaTime = ((float)(currentTick - _prevTick) / Stopwatch.Frequency);
                _prevTick = currentTick;
                _frameCnt++;
                _frameTime += Time.deltaTime;
                if (_frameTime > 1f)
                {
                    long fps = (long)(_frameCnt / _frameTime);
                    _frameCnt = 0;
                    _frameTime = 0;
                    Console.WriteLine($"fps: {fps}, delTime:{Time.deltaTime}");
                }
                roomManager.UpdateRooms();
                roomManager.FlushRooms();
            }
        }
    }
}

