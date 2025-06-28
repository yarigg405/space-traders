using UnityEngine;


namespace Assets.Code.Gameplay.CameraSystem
{
    public sealed class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform _horizontalRoot;
        [SerializeField] private float _horizontalModificator = 1f;

        [Space]
        [SerializeField] private Transform _verticalRoot;
        [SerializeField] private float _verticalModificator = 1f;

        [Space]
        [SerializeField] private Transform _zoomRoot;
        [SerializeField] private float _zoomModificator;
        [SerializeField] private Vector2 _zoomMinMax;

        private const string _mouseNameX = "Mouse X";
        private const string _mouseNameY = "Mouse Y";
        private const string _mouseScrollName = "Mouse ScrollWheel";


        private void Update()
        {
            var dt = Time.unscaledDeltaTime;

            if (Input.GetMouseButton(1))
            {
                float deltaX = Input.GetAxis(_mouseNameX);
                float deltaY = Input.GetAxis(_mouseNameY);

                var rotaionX = _horizontalRoot.localRotation.eulerAngles.y;
                rotaionX += deltaX * _horizontalModificator * dt;
                _horizontalRoot.localRotation = Quaternion.Euler(0, rotaionX, 0);

                var rotationY = _verticalRoot.localRotation.eulerAngles.x;
                rotationY += deltaY * _verticalModificator * dt;
                _verticalRoot.localRotation = Quaternion.Euler(rotationY, 0, 0);
            }

            var scrollDelta = Input.GetAxis(_mouseScrollName);
            var pos = _zoomRoot.localPosition.z;
            pos += scrollDelta * dt * _zoomModificator * Mathf.Abs(pos);
            pos = Mathf.Clamp(pos, _zoomMinMax.x, _zoomMinMax.y);

            _zoomRoot.localPosition = new Vector3(0, 0, pos);
        }
    }
}