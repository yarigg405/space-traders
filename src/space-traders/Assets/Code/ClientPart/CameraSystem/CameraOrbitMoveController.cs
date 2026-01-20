using System.Linq;
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

        [Header("Base Speeds")]
        [SerializeField] private float _wheelImpulse = 120f;
        [SerializeField] private float _analogAcceleration = 40f;

        [Header("Speed vs Radius")]
        [SerializeField] private AnimationCurve _speedByRadius = AnimationCurve.Linear(0, 1, 1, 2);
        [SerializeField] private Vector2 _speedMultiplierClamp = new(0.1f, 5f);

        [Header("Inertia")]
        [SerializeField] private float _damping = 10f;
        [SerializeField] private float _maxSpeed = 60f;
        [SerializeField] private float _stopThreshold = 0.02f;

        private float _zoomSpeed;
        private bool _hasMouseScrollBinding;


        private void OnEnable()
        {
            _look?.action.Enable();
            _orbitHold?.action.Enable();
            _zoom?.action.Enable();

            _hasMouseScrollBinding = false;
            if (_zoom?.action != null)
            {
                _hasMouseScrollBinding = _zoom.action.controls.Any(c => c.path.Contains("/scroll/")) ||
                                         _zoom.action.bindings.Any(b => b.path.Contains("/scroll/"));
            }
        }

        private void OnDisable()
        {
            _look?.action.Disable();
            _orbitHold?.action.Disable();
            _zoom?.action.Disable();
        }

        private void Update()
        {
            if (_axisController != null && _look != null)
            {
                var ld = _look.action.activeControl?.device;
                bool isGamepadLook = ld is Gamepad;
                bool holdPressed = _orbitHold != null && _orbitHold.action.IsPressed();
                _axisController.enabled = isGamepadLook || holdPressed;
            }

            if (_orbitalFollow == null || _zoom?.action == null) return;

            var range = _orbitalFollow.RadialAxis.Range;
            float min = range.x;
            float max = range.y;
            if (max <= min) return;

            float value = _orbitalFollow.RadialAxis.Value;

            float t = Mathf.InverseLerp(min, max, value);
            float mult = Mathf.Clamp(_speedByRadius.Evaluate(t), _speedMultiplierClamp.x, _speedMultiplierClamp.y);

            float input = -_zoom.action.ReadValue<float>();
            bool hasInput = Mathf.Abs(input) > 0.0001f;

            var ctrl = _zoom.action.activeControl;
            bool looksLikeScrollNow =
                ctrl != null && ctrl.path.Contains("/scroll/") ||
                ctrl == null && _hasMouseScrollBinding && Mouse.current != null;

            if (hasInput)
            {
                if (looksLikeScrollNow)
                    _zoomSpeed += input * _wheelImpulse * mult;
                else
                    _zoomSpeed += input * _analogAcceleration * mult * Time.deltaTime;
            }

            _zoomSpeed = Mathf.Clamp(_zoomSpeed, -_maxSpeed, _maxSpeed);

            value = Mathf.Clamp(value - _zoomSpeed * Time.deltaTime, min, max);

            _zoomSpeed *= Mathf.Exp(-_damping * Time.deltaTime);
            if (Mathf.Abs(_zoomSpeed) < _stopThreshold) _zoomSpeed = 0f;

            _orbitalFollow.RadialAxis.Value = value;
        }
    }
}