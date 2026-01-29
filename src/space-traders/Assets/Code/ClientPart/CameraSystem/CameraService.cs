using UnityEngine;
using VContainer.Unity;


namespace Assets.Code.ClientPart.CameraSystem
{
    public sealed class CameraService : ICameraService, ILateTickable
    {
        private readonly CameraOrbitMoveController _moveController;
        private readonly CameraTargetController _targetController;
        private readonly SkyboxCamera _skyboxCamera;

        public CameraService(CameraOrbitMoveController moveController,
            CameraTargetController cameraTargetController,
            SkyboxCamera skyboxCamera)
        {
            _moveController = moveController;
            _targetController = cameraTargetController;
            _skyboxCamera = skyboxCamera;
        }

        void ICameraService.SetTarget(Transform target)
        {
            _targetController.SetNewTarget(target);
        }

        void ILateTickable.LateTick()
        {
            _targetController.ManualUpdate();
            _skyboxCamera.ManualUpdate();
        }
    }
}
