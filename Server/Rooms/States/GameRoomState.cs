using System;

namespace Server.Rooms.States
{
    abstract class GameRoomState : IDisposable
    {
        protected GameRoom _room;
        public GameRoomState(GameRoom room)
        {
            _room = room;
        }
        public virtual void Enter() { }
        public virtual void Update() { }
        public virtual void Exit() { }

        public virtual void Dispose()
        {
            Console.WriteLine("StateDispose");
        }
    }
}
