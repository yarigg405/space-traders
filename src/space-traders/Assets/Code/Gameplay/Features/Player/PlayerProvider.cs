using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using VContainer.Unity;


namespace Assets.Code.Gameplay.Features.Player
{
    public sealed class PlayerProvider : IAsyncStartable, IPlayerProvider
    {
        public GameEntity PlayerEntity { get; private set; }
        private GameEntity _playerEntity;
        public event Action OnPlayerInitizlized;

        async UniTask IAsyncStartable.StartAsync(CancellationToken cancellation)
        {
            await UniTask.WaitUntil(() => _playerEntity != null && _playerEntity.hasTransform);
            PlayerEntity = _playerEntity;
            OnPlayerInitizlized?.Invoke();
        }

        public void SetPlayer(GameEntity playerEntity)
        {
            _playerEntity = playerEntity;
        }
    }
}
