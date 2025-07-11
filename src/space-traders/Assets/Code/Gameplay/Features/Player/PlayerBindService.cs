using Assets.Code.Gameplay.CameraSystem;
using System;
using VContainer.Unity;


namespace Assets.Code.Gameplay.Features.Player
{
    internal sealed class PlayerBindService : IStartable, IDisposable
    {
        private readonly IPlayerProvider _playerProvider;

        private readonly CameraService _cameraService;

        public PlayerBindService(IPlayerProvider playerProvider, CameraService cameraService)
        {
            _playerProvider = playerProvider;
            _cameraService = cameraService;
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
        }

        void IDisposable.Dispose()
        {
            _playerProvider.OnPlayerInitizlized -= OnPlayerChanded;
        }
    }
}
