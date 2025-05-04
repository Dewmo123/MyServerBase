using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Server.Rooms.States
{
    internal class RoomStateMachine
    {
        private Dictionary<string, GameRoomState> _states;
        private GameRoomState _currentState;
        public RoomStateMachine(GameRoom room)
        {
            _states = new Dictionary<string, GameRoomState>();
            Assembly fsmAssembly = Assembly.GetAssembly(typeof(GameRoomState));
            List<Type> types = fsmAssembly.GetTypes()
                .Where(type => !type.IsAbstract && type.IsSubclassOf(typeof(GameRoomState)))
                .ToList();
            types.ForEach(type => _states.Add(
                type.Name.Replace("State", "")
                , Activator.CreateInstance(type, room) as GameRoomState));
        }
        public void ChangeState(string name)
        {
            if (_states.TryGetValue(name, out GameRoomState state))
            {
                _currentState?.Exit();
                _currentState = state;
                state.Enter();
            }
            else
            {
                Console.WriteLine($"{name}State does not exist");
                throw new System.Exception();
            }
        }
        public void UpdateRoom()
            => _currentState.Update();
    }
}
