using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Server.Rooms.States
{
    internal class StateMachineGenerator<TOwner, TTopProduct, TEnum>
        where TEnum : Enum
        where TTopProduct : IState<TEnum>
        where TOwner : Room
    {
        private static Dictionary<string,List<Func<TOwner, TTopProduct>>> _stateFactory;

        public static void AddStateFactory(string key, string ownerParamName)
        {
            List<Type> types = new();
            _stateFactory = new();
            Assembly fsmAssembly = Assembly.GetAssembly(typeof(TTopProduct));
            types = fsmAssembly.GetTypes()
                .Where(type => !type.IsAbstract && type.IsSubclassOf(typeof(TTopProduct)))
                .ToList();
            List<Func<TOwner, TTopProduct>> factory = new();
            foreach (Type type in types)
            {
                ConstructorInfo ctor = type.GetConstructor(new[] { typeof(TOwner) });
                ParameterExpression roomParam = Expression.Parameter(typeof(TOwner), ownerParamName);
                Expression newExpr = Expression.New(ctor, roomParam);
                LambdaExpression lambda = Expression.Lambda<Func<TOwner, TTopProduct>>(newExpr, roomParam);
                factory.Add(lambda.Compile() as Func<TOwner, TTopProduct>);
            }
            _stateFactory.Add(key, factory);
        }
        public static RoomStateMachine<TOwner,TTopProduct,TEnum> GenerateMachine(TOwner owner,string key)
        {
            return new RoomStateMachine<TOwner, TTopProduct, TEnum>(owner, _stateFactory.GetValueOrDefault(key));
        }
    }
}
