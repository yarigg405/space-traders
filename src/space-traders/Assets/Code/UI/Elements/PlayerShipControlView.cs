using Assets.Code.ClientPart.Gameplay.Features.Player.Infrastructure;
using System;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;


namespace Assets.Code.UI.Elements
{
    public class PlayerShipControlView : MonoBehaviour
    {
        [SerializeField] private Slider _shipSpeedControlSlider;
        [SerializeField] private Slider _showSpeedSlider;

        private PlayerShipController _playerShipController;
        private IPlayerProvider _playerProvider;


        [Inject]
        private void Construct(PlayerShipController playerShipController, IPlayerProvider playerProvider)
        {
            _playerShipController = playerShipController;
            _playerProvider = playerProvider;
        }
        private void Start()
        {
            _shipSpeedControlSlider.value = 0;
            _shipSpeedControlSlider.onValueChanged.AddListener(OnSliderValueChanged);
        }

        private void Update()
        {
            _showSpeedSlider.value = _playerProvider.PlayerEntity.CurrentSpeedModifier;
        }

        private void OnSliderValueChanged(float arg0)
        {
            _playerShipController.SetSpeedModifier(_shipSpeedControlSlider.value);
        }

        private void OnDestroy()
        {
            _shipSpeedControlSlider.onValueChanged.RemoveListener(OnSliderValueChanged);
        }
    }
}