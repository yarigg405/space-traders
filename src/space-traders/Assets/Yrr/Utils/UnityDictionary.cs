using System;
using System.Collections.Generic;
using UnityEngine;


namespace Yrr.Utils
{
    [Serializable]
    public struct UnityDictionary<TKey, TValue>
    {
        [SerializeField] private UnityKeyValuePair<TKey, TValue>[] _data;
        private Dictionary<TKey, int> _indexes;

        public TValue Get(TKey key)
        {
            if (_indexes == null)
                Initialize();

            if (_indexes.TryGetValue(key, out var index))
                return _data[index].Value;

            Debug.Log("Item is not found: " + key.ToString());
            return default;
        }

        public bool TryGet(TKey key, out TValue value)
        {
            if (_indexes == null)
                Initialize();

            if (_indexes.TryGetValue(key, out var index))
            {
                value = _data[index].Value;
                return true;
            }

            else
            {
                value = default;
                return false;
            }
        }

        public void Initialize()
        {
            _indexes = new();
            for (int i = 0; i < _data.Length; i++)
            {
                try
                {
                    _indexes[_data[i].Key] = i;
                }

                catch
                {
                    Debug.LogError($"Item {_data[i].Key} has already been added");
                }
            }
        }
    }

    [Serializable]
    public struct UnityKeyValuePair<TKey, TValue>
    {
        public TKey Key;
        public TValue Value;
    }
}
