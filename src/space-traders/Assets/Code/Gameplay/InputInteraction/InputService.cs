using System;
using UnityEngine;
using VContainer.Unity;


namespace Assets.Code.Gameplay.InputInteraction
{
    internal sealed class InputService : IStartable, IDisposable
    {
        private readonly MouseClickNotificator _mouseClickNotificator;
        private readonly InputDataContainer _inputDataContainer;


        public InputService(MouseClickNotificator mouseClickNotificator, InputDataContainer inputDataContainer)
        {
            _mouseClickNotificator = mouseClickNotificator;
            _inputDataContainer = inputDataContainer;
        }

        void IStartable.Start()
        {
            _mouseClickNotificator.OnMouseClickEvent += OnMouseClickHandler;
        }

        void IDisposable.Dispose()
        {
            _mouseClickNotificator.OnMouseClickEvent -= OnMouseClickHandler;
        }

        private void OnMouseClickHandler(Vector3 vector)
        {
            _inputDataContainer.SetInput(vector);
        }
    }
}
