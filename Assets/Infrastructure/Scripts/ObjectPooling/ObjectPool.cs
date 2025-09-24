using System;
using System.Collections.Generic;
using UnityEngine;

namespace Infrastructure.ObjectPooling
{
    public interface IPoolable
    {
        void OnRent();
        void OnReturn();
    }

    public class ObjectPool<T> where T : Component
    {
        readonly T prefab;
        readonly Transform parent;
        readonly Stack<T> pool = new();
        readonly int maxSize;

        public int CountInactive => pool.Count;
        public int CountTotal { get; private set; }

        public ObjectPool(T prefab, int initialSize = 0, int maxSize = int.MaxValue, Transform parent = null)
        {
            this.prefab = prefab;
            this.parent = parent;
            this.maxSize = maxSize;

            for (int i = 0; i < initialSize; i++)
            {
                var obj = CreateNew();
                obj.gameObject.SetActive(false);
                pool.Push(obj);
            }
        }

        public T Rent()
        {
            T obj;
            if (pool.Count > 0)
            {
                obj = pool.Pop();
            }
            else
            {
                if (CountTotal >= maxSize)
                {
                    Debug.LogWarning($"Pool max size reached for {typeof(T).Name}");
                    return null;
                }
                obj = CreateNew();
            }

            obj.gameObject.SetActive(true);
            if (obj.TryGetComponent<IPoolable>(out var poolable))
            {
                poolable.OnRent();
            }

            return obj;
        }

        public void Return(T obj)
        {
            if (obj == null) return;

            if (obj.TryGetComponent<IPoolable>(out var poolable))
            {
                poolable.OnReturn();
            }

            obj.gameObject.SetActive(false);

            if (CountInactive >= maxSize)
            {
                GameObject.Destroy(obj.gameObject);
                CountTotal--;
                return;
            }

            pool.Push(obj);
        }

        T CreateNew()
        {
            var obj = GameObject.Instantiate(prefab, parent);
            CountTotal++;
            return obj;
        }
    }
}
