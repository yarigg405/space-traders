using Assets.Code.ClientPart.CameraSystem;
using Assets.Code.ClientPart.Gameplay.Features.Player.Infrastructure;
using Assets.Code.Common;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using VContainer.Unity;


namespace Assets.Code.ClientPart.Visual.Player
{
    internal sealed class PlayerWarpEffectController : IAsyncStartable, IDisposable, ILateTickable
    {
        private readonly PlayerWarpEffectView _warpVfx;
        private readonly IPlayerProvider _playerProvider;
        private readonly SkyboxSpaceState _skyboxSpace;

        private IDisposable _isWarpingSubscription;


        public PlayerWarpEffectController(PlayerWarpEffectView warpVfx,
            IPlayerProvider playerProvider, SkyboxSpaceState skyboxSpace)
        {
            _warpVfx = warpVfx;
            _playerProvider = playerProvider;
            _skyboxSpace = skyboxSpace;
        }


        async UniTask IAsyncStartable.StartAsync(CancellationToken cancellation)
        {
            await UniTask.WaitUntil(() =>
            _playerProvider.PlayerEntity != null,
            cancellationToken: cancellation);

            if (cancellation.IsCancellationRequested)
                return;

            _isWarpingSubscription = _playerProvider.PlayerEntity.ViewModel.IsWarping.Subscribe(OnWarpStateChanged, true);
        }

        void ILateTickable.LateTick()
        {
            if (_playerProvider.PlayerEntity == null) return;

            var entity = _playerProvider.PlayerEntity;

            _warpVfx.gameObject.SetActive(true);

            _warpVfx.transform.rotation = Quaternion.Euler(0, entity.CurrentRotationY, 0);


            var globalPosition = entity.GlobalPosition;
            var anchor = _skyboxSpace.SkyboxAnchor;

            var dx = globalPosition.x - anchor.x;
            var dy = globalPosition.y - anchor.y;

            float scale = GameConstants.SKYBOX_OBJECTS_POSITION_MODIFIER;

            _warpVfx.transform.position = new Vector3(
            (float)(dx * scale),
            0f,
            (float)(dy * scale));
        }

        void IDisposable.Dispose()
        {
            _isWarpingSubscription?.Dispose();
            _isWarpingSubscription = null;
        }


        private void OnWarpStateChanged(bool isWarping)
        {
            if (isWarping)
            {
                _warpVfx.ShowWarp();
            }

            else
            {
                _warpVfx.HideWarp();
            }
        }
    }
}
