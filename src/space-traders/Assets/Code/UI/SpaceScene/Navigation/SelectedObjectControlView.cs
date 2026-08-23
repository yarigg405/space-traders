using Assets.Code.ClientPart.CameraSystem;
using Assets.Code.ClientPart.Gameplay.Features.Navigation;
using Assets.Code.ClientPart.Gameplay.Features.Player.Infrastructure;
using Assets.Code.Common;
using Assets.Code.Common.Time;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;


namespace Assets.Code.UI.SpaceScene.Navigation
{
    public sealed class SelectedObjectControlView : MonoBehaviour
    {
        [SerializeField] private Image _selectedObjectIcon;
        [SerializeField] private TextMeshProUGUI _selectedObjectNameTmp;
        [SerializeField] private TextMeshProUGUI _selectedObjectDistanceTmp;

        [Space]
        [SerializeField] private Button _approachBtn;
        [SerializeField] private Button _warpBtn;
        [SerializeField] private Button _setCameraTargetBtn;
        [SerializeField] private Button _distanceBtn;
        [SerializeField] private Button _orbitBtn;
        [SerializeField] private TMP_InputField _distanceInputField;

        [Space]
        [SerializeField] private float _distanceRefreshInterval = 0.1f;

        private PlayerShipController _playerShipController;
        private ICameraService _cameraService;
        private SelectionService _selectionService;
        private IPlayerProvider _playerProvider;
        private ITimeService _time;

        private GameEntity _currentSelected;
        private float _timer;
        private const float _distanceThreshold = 7.5f;


        [Inject]
        private void Construct(PlayerShipController playerShipController, ICameraService cameraService,
            SelectionService selectionService, IPlayerProvider playerProvider, ITimeService time)
        {
            _playerShipController = playerShipController;
            _cameraService = cameraService;
            _selectionService = selectionService;
            _playerProvider = playerProvider;
            _time = time;
        }

        private void OnEnable()
        {
            _approachBtn.onClick.AddListener(ClickOnApproach);
            _warpBtn.onClick.AddListener(ClickOnWarp);
            _setCameraTargetBtn.onClick.AddListener(ClickOnSetCamera);
            _distanceBtn.onClick.AddListener(ClickOnSetDistance);
            _orbitBtn.onClick.AddListener(ClickOnSetOrbit);

            _selectionService.SelectionChanged += OnSelectionChanged;
            OnSelectionChanged(_selectionService.Selected);
        }

        private void OnDisable()
        {
            _approachBtn.onClick.RemoveListener(ClickOnApproach);
            _warpBtn.onClick.RemoveListener(ClickOnWarp);
            _setCameraTargetBtn.onClick.RemoveListener(ClickOnSetCamera);
            _distanceBtn.onClick.RemoveListener(ClickOnSetDistance);
            _orbitBtn.onClick.RemoveListener(ClickOnSetOrbit);

            _selectionService.SelectionChanged -= OnSelectionChanged;
        }

        private void Update()
        {
            _timer -= _time.DeltaTime;
            if (_timer > 0) return;

            _timer = _distanceRefreshInterval;
            RefreshDistance();
        }

        private void OnSelectionChanged(GameEntity selected)
        {
            _currentSelected = selected;
            _selectedObjectNameTmp.text = selected.GetName();
            RefreshDistance();
        }

        private void RefreshDistance()
        {
            if (_currentSelected == null)
            {
                _selectedObjectDistanceTmp.text = string.Empty;
                return;
            }

            var player = _playerProvider.PlayerEntity;
            if (player == null || !player.hasGlobalPosition) return;

            if (_currentSelected.TryGetUiDistance(player.GlobalPosition, out var distance))
                _selectedObjectDistanceTmp.text = DistanceFormat.Format(distance);
        }

        private void ClickOnApproach()
        {
            if (_currentSelected == null) return;

            var minMaxDistance = new Vector2(0, _distanceThreshold);
            _playerShipController.SetKeepDistance(_currentSelected, minMaxDistance);
        }

        private void ClickOnWarp()
        {
            if (_currentSelected == null) return;

            _playerShipController.SetWarpToEntity(_currentSelected);
        }

        private void ClickOnSetCamera()
        {
            if (_currentSelected == null || !_currentSelected.hasView) return;

            _cameraService.SetTarget(_currentSelected.View.transform);
        }

        private void ClickOnSetDistance()
        {
            if (_currentSelected == null) return;

            var distance = GetDistance();
            var minMaxDistance = new Vector2(distance - _distanceThreshold, distance + _distanceThreshold);
            _playerShipController.SetKeepDistance(_currentSelected, minMaxDistance);
        }

        private void ClickOnSetOrbit()
        {
            if (_currentSelected == null) return;

            var orbitRadius = GetDistance();
            _playerShipController.SetOrbit(_currentSelected, orbitRadius);
        }

        private float GetDistance()
        {
            if (float.TryParse(_distanceInputField.text, out var distance))
                return distance * GameConstants.DISTANCE_UI_TO_REAL;

            return 0;
        }
    }
}
