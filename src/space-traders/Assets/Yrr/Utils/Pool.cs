using System.Collections.Generic;
using UnityEngine;


namespace Yrr.Utils
{
    public class Pool<T> : MonoBehaviour where T : Component
    {
        [SerializeField] private T _prefab;
        [SerializeField] private Transform _objectsInPoolContainer;
        [SerializeField] private int _initializeCount = 10;

        private readonly LinkedList<T> _pooledObjects = new();

        private void Awake()
        {
            InitializePool(_prefab, _initializeCount);
        }

        private void InitializePool(T prefab, int initialCount)
        {
            for (var i = 0; i < initialCount; i++)
            {
                var bullet = Instantiate(prefab, _objectsInPoolContainer);
                _pooledObjects.AddLast(bullet);
            }
        }

        public T SpawnObject(Transform parentForNewObject = null)
        {
            T newObject = null;

            if (_pooledObjects.Count > 0)
            {
                newObject = _pooledObjects.First.Value;
                _pooledObjects.RemoveFirst();
                newObject.transform.SetParent(parentForNewObject);
            }
            else
            {
                newObject = Instantiate(_prefab, parentForNewObject);
            }

            return newObject;
        }

        public void DespawnObject(T poolableObject)
        {
            if (!poolableObject) return;

            poolableObject.transform.SetParent(_objectsInPoolContainer);
            _pooledObjects.AddLast(poolableObject);
        }

        public IEnumerable<T> GetPooledObjects()
        {
            return _pooledObjects;
        }
    }
}
