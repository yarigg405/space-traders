using Assets.Code.ClientPart.CameraSystem;
using Assets.Code.ClientPart.View.Factory;
using Assets.Code.Infrastructure.Loading;
using UnityEngine;
using VContainer;


namespace Assets.Code.ClientPart.View
{
    internal sealed class StationShipView : MonoBehaviour
    {
        [SerializeField] private Transform _shipAnchor;

        private IShipViewFactory _shipViewFactory;
        private StationSceneDataHolder _dataHolder;
        private ICameraService _cameraService;
        private GameObject _currentModel;

        [Inject]
        public void Construct(IShipViewFactory shipViewFactory,
            StationSceneDataHolder dataHolder, ICameraService cameraService)
        {
            _shipViewFactory = shipViewFactory;
            _dataHolder = dataHolder;
            _cameraService = cameraService;
            _dataHolder.Changed += OnStationDataChanged;
        }

        private void OnDestroy()
        {
            if (_dataHolder != null)
                _dataHolder.Changed -= OnStationDataChanged;
        }

        private void OnStationDataChanged()
        {
            Rebuild(_dataHolder.Current.ShipModelId);
        }

        private void Rebuild(string shipModelId)
        {
            if (_currentModel != null)
                Destroy(_currentModel);

            _currentModel = _shipViewFactory.CreateShipModel(shipModelId, _shipAnchor);
            _cameraService.SetTarget(_currentModel.transform);
        }
    }
}
