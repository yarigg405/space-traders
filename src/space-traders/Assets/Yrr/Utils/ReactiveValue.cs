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

        public IDisposable Subscribe(Action<T> listener, bool invokeImmediately = false)
        {
            OnChange += listener;

            if (invokeImmediately)
                listener(_currentValue);

            return new Subscription(this, listener);
        }

        private void Unsubscribe(Action<T> listener)
        {
            OnChange -= listener;
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


        private sealed class Subscription : IDisposable
        {
            private ReactiveValue<T> _source;
            private Action<T> _listener;

            public Subscription(ReactiveValue<T> reactiveValue, Action<T> listener)
            {
                _source = reactiveValue;
                _listener = listener;
            }

            void IDisposable.Dispose()
            {
                if (_source == null) return;

                _source.Unsubscribe(_listener);

                _source = null;
                _listener = null;
            }
        }
    }
}
