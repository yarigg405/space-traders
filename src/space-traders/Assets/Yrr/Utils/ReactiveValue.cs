using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace Yrr.Utils
{
    [Serializable]
    public sealed class ReactiveValue<T>
    {
        public event Action<T> OnChange;

        [SerializeField]
#if UNITY_EDITOR
        [ReadOnly]
#endif
        private T _currentValue;

        public T Value
        {
            get => _currentValue;
            set => SetValue(value);
        }


        public void SetValue(T value)
        {
            if (EqualityComparer<T>.Default.Equals(_currentValue, value))
                return;

            _currentValue = value;
            OnChange?.Invoke(_currentValue);
        }

        public void Cleanup()
        {
            OnChange = null;
        }


        public static implicit operator T(ReactiveValue<T> value)
        {
            return value.Value;
        }



        public override bool Equals(object obj)
        {
            return Value.Equals(obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Value);
        }
    }
}
