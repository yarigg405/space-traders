using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Assets.Code.ClientPart.CameraSystem
{
    public sealed class CameraOrbitMoveController : MonoBehaviour
    {
        [Header("Cinemachine")]
        [SerializeField] private CinemachineInputAxisController _axisController;
        [SerializeField] private CinemachineOrbitalFollow _orbitalFollow;

        [Header("Input Actions")]
        [SerializeField] private InputActionReference _look;
        [SerializeField] private InputActionReference _orbitHold;
        [SerializeField] private InputActionReference _zoom;

        [Header("Values")]
        [SerializeField] private float _zoomAcceleration;
        [SerializeField] private float _zoomDeceleration;

        private float _currentZoomSpeed;

        private Vector2 _zoomRange;


        private void OnEnable()
        {
            _zoomRange = _orbitalFollow.RadialAxis.Range;

            _look?.action.Enable();
            _orbitHold?.action.Enable();
            _zoom?.action.Enable();
        }

        private void OnDisable()
        {
            _look?.action.Disable();
            _orbitHold?.action.Disable();
            _zoom?.action?.Disable();
        }

        private void Update()
        {
            HandleLook();
            HandleZoom();
        }

        private void HandleLook()
        {
            var ld = _look.action.activeControl?.device;
            bool isGamepadLook = ld is Gamepad;
            bool holdPressed = _orbitHold.action.IsPressed();
            _axisController.enabled = isGamepadLook || holdPressed;
        }

        private void HandleZoom()
        {
            var deltaTime = Time.unscaledDeltaTime;

            float input = _zoom.action.ReadValue<float>();
            _currentZoomSpeed += input * _zoomAcceleration * deltaTime;


            var value = _orbitalFollow.RadialAxis.Value + _currentZoomSpeed * deltaTime;
            value = Mathf.Clamp(value, _zoomRange.x, _zoomRange.y);

            _orbitalFollow.RadialAxis.Value = value;

            _currentZoomSpeed = Mathf.MoveTowards(_currentZoomSpeed, 0f, _zoomDeceleration * deltaTime);
        }
    }
}