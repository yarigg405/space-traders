using UnityEngine;


namespace Assets.Code.Gameplay.CameraSystem
{
    internal class CameraService
    {
        private readonly CameraMover _mover;
        private readonly CameraController _controller;

        public CameraService(CameraMover mover, CameraController controller)
        {
            _mover = mover;
            _controller = controller;
        }

        public void SetTarget(Transform target)
        {
            _mover.SetNewTarget(target);
        }
    }
}
