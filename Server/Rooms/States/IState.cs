namespace Server.Rooms.States
{
    internal interface IState<TEnum>
    {
        /// <summary>
        /// 각각의 스테이트 타입별로 가지고 있어야하는 열거형 입니다. 같은 머신에서 중복된 내용을 가지면 안됩니다.
        /// </summary>
        TEnum EnumType { get; }
        void Enter();
        void Update();
        void Exit();
    }
}
