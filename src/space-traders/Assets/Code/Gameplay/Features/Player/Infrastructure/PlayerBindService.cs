using Assets.Code.Gameplay.CameraSystem;
using Assets.Code.UI.Elements;
using System;
using VContainer.Unity;


namespace Assets.Code.Gameplay.Features.Player.Infrastructure
{
    internal sealed class PlayerBindService : IStartable, IDisposable
    {
        private readonly IPlayerProvider _playerProvider;

        private readonly CameraService _cameraService;
        private readonly PlayerShipControlView _playerShipControlView;

        public PlayerBindService(IPlayerProvider playerProvider, 
            CameraService cameraService, PlayerShipControlView playerShipControlView)
        {
            _playerProvider = playerProvider;
            _cameraService = cameraService;
            _playerShipControlView = playerShipControlView;
        }

        void IStartable.Start()
        {
            _playerProvider.OnPlayerInitizlized += OnPlayerChanded;
            if (_playerProvider.PlayerEntity != null)
                OnPlayerChanded();
        }

        private void OnPlayerChanded()
        {
            if (_playerProvider.PlayerEntity == null) return;

            _cameraService.SetTarget(_playerProvider.PlayerEntity.Transform);
            _playerShipControlView.SetupPlayer(_playerProvider.PlayerEntity);
        }

        void IDisposable.Dispose()
        {
            _playerProvider.OnPlayerInitizlized -= OnPlayerChanded;

            _playerShipControlView.Dispose();
        }
    }
}
