using Server.Utiles;

namespace Server.Rooms.States
{
    internal class GameEndState : SyncObjectsState
    {

        private CountTime _endCount;
        private static readonly int _endTime = 5;
        public GameEndState(GameRoom room) : base(room)
        {
            _endCount = new CountTime(HandleElapsed, OnCountEnd, _endTime, 100);
        }
        public override void Enter()
        {
            base.Enter();
            _endCount.StartCount();
        }
        private void OnCountEnd()
        {
            S_LeaveRoom leavePacket = new();

            foreach (var item in _room.Sessions)
            {
                item.Value.Send(leavePacket.Serialize());
                item.Value.Room = null;
            }
            _room.AllPlayerExit();
        }
        S_SyncTimer _timerPacket = new();
        private void HandleElapsed(double obj)
        {
            _timerPacket.time = (float)(_endTime - obj);
            _room.Broadcast(_timerPacket);
        }
        public override void Dispose()
        {
            base.Dispose();
            if (_endCount.IsRunning)
                _endCount.Abort();
        }
    }
}
