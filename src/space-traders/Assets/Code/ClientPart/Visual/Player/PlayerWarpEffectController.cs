using Assets.Code.ClientPart.CameraSystem;
using Assets.Code.ClientPart.Gameplay.Features.Player.Infrastructure;
using Assets.Code.Common;
using System;
using UnityEngine;
using VContainer.Unity;


namespace Assets.Code.ClientPart.Visual.Player
{
    internal sealed class PlayerWarpEffectController : IStartable, IDisposable, ILateTickable
    {
        private readonly PlayerWarpEffectView _warpVfx;
        private readonly IPlayerProvider _playerProvider;
        private readonly SkyboxSpaceState _skyboxSpace;
        private readonly PlayerViewModel _viewModel;


        public PlayerWarpEffectController(PlayerWarpEffectView warpVfx,
            IPlayerProvider playerProvider, SkyboxSpaceState skyboxSpace,
            PlayerViewModel viewModel)
        {
            _warpVfx = warpVfx;
            _playerProvider = playerProvider;
            _skyboxSpace = skyboxSpace;
            _viewModel = viewModel;
        }


        void IStartable.Start()
        {
            _viewModel.PlayerIsWarping.OnChange += OnWarpStateChanged;
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

            var newLocal = new Vector3(
                (float)(dx * scale),
                0f,
                (float)(dy * scale)
            );

            _warpVfx.transform.position = newLocal;
        }

        void IDisposable.Dispose()
        {
            _viewModel.PlayerIsWarping.OnChange -= OnWarpStateChanged;
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
