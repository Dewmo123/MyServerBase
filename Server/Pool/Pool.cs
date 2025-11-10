using Server.Pool;
using Server.Utiles;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Server.Events
{
    internal interface IObjectPool
    {
        void Clear();
    }

    internal class Pool<T> : IObjectPool where T : IPoolable, new()
    {
        private ConcurrentBag<T> _pool = new();
        public void Clear()
        {
            _pool.Clear();
        }

        public T Pop()
        {
            if(_pool.TryTake(out T val))
                return val;
            return new T();
        }
        public void Push(T val)
        {
            val.ResetItem();
            _pool.Add(val);
        }
    }
}
