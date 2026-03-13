using ServerCore.Utiles;
using System;
using System.Collections.Concurrent;

namespace ServerCore.PoolManage
{
    public class PoolManager : Singleton<PoolManager>
    {
        private ConcurrentDictionary<Type, IObjectPool> _pools = new();
        
        public T Pop<T>() where T : IPoolable,new()
        {
            var pool = _pools.GetOrAdd(typeof(T), _ => new Pool<T>()) as Pool<T>;
            return pool.Pop();
        }
        public void Push<T>(T val) where T : IPoolable, new()
        {
            var pool = _pools.GetOrAdd(typeof(T), _ => new Pool<T>()) as Pool<T>;
            pool.Push(val);
        }
        public void ClearAllPool()
        {
            foreach (var pool in _pools.Values)
                pool.Clear();
            _pools.Clear();
        }
    }
}
