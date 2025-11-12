using Server.Utiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Server.Rooms.States
{
    internal class StateMachine<TOwner,TTopProduct,TEnum>
        where TEnum : Enum 
        where TTopProduct : IState<TEnum>
        where TOwner : Room
    {
        private Dictionary<TEnum, TTopProduct> _states;
        public TTopProduct CurrentState { get; private set; }
        public TEnum CurrentStateEnum { get; private set; }
        public StateMachine(TOwner room,List<Func<TOwner,TTopProduct>> stateFactory)
        {
            _states = new();
            foreach(var item in stateFactory)
            {
                TTopProduct product = item.Invoke(room);
                _states.Add(product.EnumType, product);
            }
        }

        public void ChangeState(TEnum type)
        {
            Console.WriteLine($"ChangeState: {type}");
            if (_states.TryGetValue(type, out TTopProduct state))
            {
                CurrentState?.Exit();
                CurrentStateEnum = type;
                CurrentState = state;
                state.Enter();
            }
            else
            {
                Console.WriteLine($"{type}State does not exist");
                throw new System.Exception();
            }
        }
        public void UpdateRoom()
            => CurrentState.Update();
    }
}
