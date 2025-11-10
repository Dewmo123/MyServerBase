using Server.Utiles;

namespace Server.Rooms.States
{
    internal class LobbyState : GameRoomState
    {
        public override RoomState EnumType => RoomState.Lobby;
        public LobbyState(GameRoom room) : base(room)
        {
        }

    }
}
