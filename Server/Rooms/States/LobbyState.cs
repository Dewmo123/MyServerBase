using System;

namespace Server.Rooms.States
{
    class LobbyState : GameRoomState
    {
        public LobbyState(GameRoom room) : base(room)
        {
        }

        public override void Update()
        {
            base.Update();
            if (!_room.CanAddPlayer)
                _room.ChangeState("Prepare");
        }
    }
}
