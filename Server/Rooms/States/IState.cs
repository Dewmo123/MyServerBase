namespace Server.Rooms.States
{
    internal interface IState<TEnum>
    {
        TEnum EnumType { get; }
        void Enter();
        void Update();
        void Exit();
    }
}
