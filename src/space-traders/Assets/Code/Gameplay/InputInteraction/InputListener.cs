using System;
using UnityEngine;
using VContainer.Unity;


namespace Assets.Code.Gameplay.InputInteraction
{
    internal sealed class InputListener : IStartable, IDisposable
    {
        private readonly MouseClickNotificator _mouseClickNotificator;

        public InputListener(MouseClickNotificator mouseClickNotificator)
        {
            _mouseClickNotificator = mouseClickNotificator;
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
            Debug.Log("### Clicked in: " + vector);
        }
    }
}
