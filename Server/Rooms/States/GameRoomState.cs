using Server.Utiles;
using System;

namespace Server.Rooms.States
{
    abstract class GameRoomState : IState<RoomState>
    {
        protected GameRoom _room;

        public abstract RoomState EnumType { get; }

        public GameRoomState(GameRoom room)
        {
            _room = room;
        }
        public virtual void Enter() { }
        public virtual void Update() { }
        public virtual void Exit() { }
    }
}
