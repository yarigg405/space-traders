using Assets.Code.Common;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using VContainer.Unity;


namespace Assets.Code.ClientPart.Gameplay.Features.InputInteraction
{
    internal sealed class MouseClickDetector : IInitializable, IDisposable
    {
        private readonly CameraRaycaster _raycaster;
        private readonly InputReferencesContainer _inputReferencesContainer;
        private readonly Contexts _ctx;

        public event Action<Vector3> OnMouseClickEvent;
        public event Action<ClickableEntity> OnObjectClicked;


        public MouseClickDetector(CameraRaycaster raycaster,
            InputReferencesContainer inputReferencesContainer,
            Contexts ctx)
        {
            _raycaster = raycaster;
            _inputReferencesContainer = inputReferencesContainer;
            _ctx = ctx;
        }

        void IInitializable.Initialize()
        {
            _inputReferencesContainer.DoubleClick.action.Enable();
            _inputReferencesContainer.DoubleClick.action.performed += OnDoubleClick;
        }

        void IDisposable.Dispose()
        {
            _inputReferencesContainer.DoubleClick.action.performed -= OnDoubleClick;
            _inputReferencesContainer.DoubleClick.action.Disable();
        }


        private void OnDoubleClick(InputAction.CallbackContext ctx)
        {
            if (_raycaster.RaycastFromCameraToMouse(out var raycastHit))
            {
                if (EventSystem.current.IsPointerOverGameObject(Mouse.current.deviceId)) return;

                var clickable = raycastHit.collider.gameObject.GetComponent<ClickableEntity>();
                if (clickable)
                {
                    OnObjectClicked?.Invoke(clickable);
                    return;
                }

                var clickPos = raycastHit.point;
                OnMouseClickEvent?.Invoke(clickPos);

                CreateEntity.EmptyInput(_ctx)
                    .AddClickedPosition(clickPos)
                    .isInput = true;
            }
        }
    }
}
