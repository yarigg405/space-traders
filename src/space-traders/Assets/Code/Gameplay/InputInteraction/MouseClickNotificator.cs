using System;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer.Unity;


namespace Assets.Code.Gameplay.InputInteraction
{
    internal sealed class MouseClickNotificator : ITickable
    {
        private readonly CameraRaycaster _raycaster;

        public event Action<Vector3> OnMouseClickEvent;

        public MouseClickNotificator(CameraRaycaster raycaster)
        {
            _raycaster = raycaster;
        }

        void ITickable.Tick()
        {
            if (UnityEngine.Input.GetMouseButtonUp(0))
            {
                if (_raycaster.RaycastFromCameraToMouse(out var raycastHit))
                {
                    if (EventSystem.current.IsPointerOverGameObject()) return;
                    var clickPos = raycastHit.point;
                    OnMouseClickEvent?.Invoke(clickPos);
                }
            }
        }
    }
}
