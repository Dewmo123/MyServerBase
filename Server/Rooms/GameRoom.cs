using Server.Objects;
using Server.Rooms.States;
using Server.Utiles;

namespace Server.Rooms
{
    internal class GameRoom : Room
    {
        private StateMachine<GameRoom, GameRoomState, RoomState> _stateMachine;
        public RoomState CurrentState => _stateMachine.CurrentStateEnum;

        public GameRoom(RoomManager manager, int roomId, string name) : base(manager, roomId, name)
        {
        }

        #region StateMachine


        public void ChangeState(RoomState state)
        {
            _stateMachine.ChangeState(state);
        }
        public override void UpdateRoom()
            => _stateMachine?.UpdateRoom();

        public override void ObjectDead(ObjectBase obj)
        {
        }
        #endregion
    }
}
