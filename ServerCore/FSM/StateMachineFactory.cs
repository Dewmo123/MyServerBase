using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ServerCore.FSM
{
    /// <summary>
    /// StateMachine 생성해주는 클래스입니다.
    /// </summary>
    /// <typeparam name="TOwner">State들에게 주입될 객체 타입입니다.</typeparam>
    /// <typeparam name="TTopProduct">가장 상위인 추상 클래스 타입입니다. IState를 구현해야 합니다.</typeparam>
    /// <typeparam name="TEnum">State들을 관리할 Enum 타입입니다.</typeparam>
    public class StateMachineFactory<TOwner, TTopProduct, TEnum>
        where TEnum : Enum
        where TTopProduct : IState<TEnum>
        where TOwner : class
    {
        private static Dictionary<string, List<Func<TOwner, TTopProduct>>> _stateFactory = new();

        public static void AddStateFactory(string key, string ownerParamName)
        {
            if (_stateFactory.ContainsKey(key))
                throw new ArgumentException("동일한 key가 이미 있는 상태");
            List<Type> types = new();
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
        public static StateMachine<TOwner, TTopProduct, TEnum> GenerateMachine(TOwner owner, string key)
        {
            if (_stateFactory.TryGetValue(key, out List<Func<TOwner, TTopProduct>> factory))
                return new StateMachine<TOwner, TTopProduct, TEnum>(owner, factory);
            else
                throw new NullReferenceException();
        }
    }
}
