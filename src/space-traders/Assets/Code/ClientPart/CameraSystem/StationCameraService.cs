using UnityEngine;
using VContainer.Unity;


namespace Assets.Code.ClientPart.CameraSystem
{
    public sealed class StationCameraService : ICameraService, ILateTickable
    {
        private readonly CameraTargetController _targetController;

        public StationCameraService(CameraTargetController targetController)
        {
            _targetController = targetController;
        }

        void ICameraService.SetTarget(Transform target)
        {
            _targetController.SetNewTarget(target);
        }

        void ILateTickable.LateTick()
        {
            _targetController.ManualUpdate();
        }
    }
}
