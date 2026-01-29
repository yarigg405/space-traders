using Assets.Code.ClientPart.Gameplay.Features.Player.Infrastructure;
using Assets.Code.Common;
using Unity.Mathematics;
using UnityEngine;
using VContainer;


namespace Assets.Code.ClientPart.CameraSystem
{
    public sealed class SkyboxCamera : MonoBehaviour
    {
        [SerializeField] private Transform _skyboxCameraRoot;
        [Inject] private readonly IPlayerProvider _playerProvider;
        [Inject] private readonly SkyboxSpaceState _skyboxSpaceState;


        private Camera _mainCamera;
        private Camera MainCamera
        {
            get
            {
                if (!_mainCamera)
                    _mainCamera = Camera.main;
                return _mainCamera;
            }
        }

        internal void ManualUpdate()
        {
            if (_playerProvider.PlayerEntity == null) return;

            UpdateCameraPosition(_playerProvider.PlayerEntity.GlobalPosition);
            UpdateCameraRotation();
        }

        private void UpdateCameraPosition(double2 playerPosition)
        {
            double dx = playerPosition.x - _skyboxSpaceState.SkyboxAnchor.x;
            double dy = playerPosition.y - _skyboxSpaceState.SkyboxAnchor.y;

            float scale = GameConstants.SKYBOX_OBJECTS_POSITION_MODIFIER;

            _skyboxCameraRoot.position = new Vector3(
            (float)(dx * scale),
            _skyboxCameraRoot.position.y,
            (float)(dy * scale));
        }

        private void UpdateCameraRotation()
        {
            _skyboxCameraRoot.rotation = MainCamera.transform.rotation;
        }
    }
}