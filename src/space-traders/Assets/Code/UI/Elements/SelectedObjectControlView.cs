using Assets.Code.ClientPart.CameraSystem;
using Assets.Code.ClientPart.Gameplay.Features.InputInteraction;
using Assets.Code.ClientPart.Gameplay.Features.Player.Infrastructure;
using Assets.Code.Common;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using VContainer;


namespace Assets.Code.UI.Elements
{
    public sealed class SelectedObjectControlView : MonoBehaviour
    {
        [SerializeField] private Image _selectedObjectIcon;
        [SerializeField] private TextMeshProUGUI _selectedObjectTmp;

        [Space]
        [SerializeField] private Button _approachBtn;
        [SerializeField] private Button _warpBtn;
        [SerializeField] private Button _setCameraTargetBtn;
        [SerializeField] private Button _distanceBtn;
        [SerializeField] private Button _orbitBtn;
        [SerializeField] private TMP_InputField _distanceInputField;
        [SerializeField] private TMP_InputField _warpCoordinatesIF_X;
        [SerializeField] private TMP_InputField _warpCoordinatesIF_Y;


        private PlayerShipController _playerShipController;
        private ICameraService _cameraService;
        private MouseClickDetector _mouseClickDetector;

        private ClickableEntity _currentSelected;
        private const float _distanceThreshold = 7.5f;


        [Inject]
        private void Construct(PlayerShipController playerShipController, ICameraService cameraService,
            MouseClickDetector mouseClickNotificator)
        {
            _playerShipController = playerShipController;
            _cameraService = cameraService;
            _mouseClickDetector = mouseClickNotificator;
        }

        private void OnEnable()
        {
            _approachBtn.onClick.AddListener(ClickOnApproach);
            _warpBtn.onClick.AddListener(ClickOnWarp);
            _setCameraTargetBtn.onClick.AddListener(ClickOnSetCamera);
            _distanceBtn.onClick.AddListener(ClickOnSetDistance);
            _orbitBtn.onClick.AddListener(ClickOnSetOrbit);

            _mouseClickDetector.OnObjectClicked += OnObjectClickedHandler;
        }

        private void OnDisable()
        {
            _approachBtn.onClick.RemoveListener(ClickOnApproach);
            _warpBtn.onClick.RemoveListener(ClickOnWarp);
            _setCameraTargetBtn.onClick.RemoveListener(ClickOnSetCamera);
            _distanceBtn.onClick.RemoveListener(ClickOnSetDistance);
            _orbitBtn.onClick.RemoveListener(ClickOnSetOrbit);

            _mouseClickDetector.OnObjectClicked -= OnObjectClickedHandler;
        }

        private void OnObjectClickedHandler(ClickableEntity clicked)
        {
            _currentSelected = clicked;
            _selectedObjectTmp.text = clicked.GetHashCode().ToString();
        }

        private void ClickOnApproach()
        {
            var minMaxDistance = new Vector2(0, _distanceThreshold);
            _playerShipController.SetKeepDistance(_currentSelected, minMaxDistance);
        }

        private void ClickOnWarp()
        {
            var coordinates = GetWarpCoordinates();
            _playerShipController.SetWarpTo(coordinates);
        }

        private void ClickOnSetCamera()
        {
            _cameraService.SetTarget(_currentSelected.transform);
        }

        private void ClickOnSetDistance()
        {
            var distance = GetDistance();
            var minMaxDistance = new Vector2(distance - _distanceThreshold, distance + _distanceThreshold);
            _playerShipController.SetKeepDistance(_currentSelected, minMaxDistance);
        }

        private void ClickOnSetOrbit()
        {
            var orbitRadius = GetDistance();
            _playerShipController.SetOrbit(_currentSelected, orbitRadius);
        }

        private float GetDistance()
        {
            if (float.TryParse(_distanceInputField.text, out var distance))
                return distance * GameConstants.DISTANCE_UI_TO_REAL;

            return 0;
        }

        private double2 GetWarpCoordinates()
        {
            if (double.TryParse(_warpCoordinatesIF_X.text, out var warpCoordinatesX) &&
               double.TryParse(_warpCoordinatesIF_Y.text, out var warpCoordinatesY))
            {
                return new(warpCoordinatesY, warpCoordinatesX);
            }

            return new(0, 0);
        }
    }
}