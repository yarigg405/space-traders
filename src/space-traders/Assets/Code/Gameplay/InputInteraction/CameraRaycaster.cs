using UnityEngine;


namespace Assets.Code.Gameplay.InputInteraction
{
    internal sealed class CameraRaycaster
    {
        private const float _rayDistance = 1000f;
        private const string _layerName = "ClickPanel";
        private readonly LayerMask _layerMask;

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

        public CameraRaycaster()
        {
            _layerMask = LayerMask.NameToLayer(_layerName);
        }

        internal bool RaycastFromCameraToMouse(out RaycastHit hit)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, _rayDistance, _layerMask))
            {
                return true;
            }

            return false;
        }
    }
}
