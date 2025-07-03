using UnityEngine;


namespace Assets.Code.Gameplay.InputInteraction
{
    internal sealed class CameraRaycaster
    {
        private const float _rayDistance = 1000f;
        private readonly LayerMask _layerMask = LayerMask.GetMask("ClickPanel");

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
            Ray ray = _camera.ScreenPointToRay(UnityEngine.Input.mousePosition);
            if (Physics.Raycast(ray, out hit, _rayDistance, _layerMask))
            {
                return true;
            }

            return false;
        }
    }
}
