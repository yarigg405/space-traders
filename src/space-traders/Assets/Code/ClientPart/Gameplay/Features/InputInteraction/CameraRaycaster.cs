using UnityEngine;
using UnityEngine.InputSystem;


namespace Assets.Code.ClientPart.Gameplay.Features.InputInteraction
{
    internal sealed class CameraRaycaster
    {
        private const float _rayDistance = 10_000f;
        private readonly LayerMask _layerMask = LayerMask.GetMask("ClickPanel", "Default");

        private Camera _mainCamera;
        private Camera _camera
        {
            get
            {
                if (!_mainCamera)
                    _mainCamera = Camera.main;

                return _mainCamera;
            }
        }

        internal bool RaycastFromCameraToMouse(out RaycastHit hit)
        {
            hit = default;

            if (Mouse.current == null)
                return false;

            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Ray ray = _camera.ScreenPointToRay(mousePosition);
            return Physics.Raycast(ray, out hit, _rayDistance, _layerMask);
        }
    }
}