using UnityEngine;


namespace Assets.Code.ClientPart.CameraSystem
{
    public sealed class CameraService : ICameraService
    {
        private readonly CameraOrbitMoveController _moveController;
        private readonly CameraTargetController _targetController;

        public CameraService(CameraOrbitMoveController moveController,
            CameraTargetController cameraTargetController)
        {
            _moveController = moveController;
            _targetController = cameraTargetController;
        }

        void ICameraService.SetTarget(Transform target)
        {
            _targetController.SetNewTarget(target);
        }
    }
}
