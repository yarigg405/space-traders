using Assets.Code.Common;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using VContainer.Unity;


namespace Assets.Code.ClientPart.Gameplay.Features.InputInteraction
{
    internal sealed class MouseClickDetector : IInitializable, IDisposable, ITickable
    {
        private readonly CameraRaycaster _raycaster;
        private readonly InputReferencesContainer _inputReferencesContainer;
        private readonly Contexts _ctx;

        public event Action<Vector3> OnMouseClickEvent;
        public event Action<ClickableEntity> OnObjectClicked;

        private bool _clickRequested;

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

        void ITickable.Tick()
        {
            if (!_clickRequested) return;

            _clickRequested = false;

            if (EventSystem.current.IsPointerOverGameObject())
                return;

            PerformDoubleClick();
        }


        private void OnDoubleClick(InputAction.CallbackContext ctx)
        {
            _clickRequested = true;
        }

        private void PerformDoubleClick()
        {
            if (_raycaster.RaycastFromCameraToMouse(out var raycastHit))
            {
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
