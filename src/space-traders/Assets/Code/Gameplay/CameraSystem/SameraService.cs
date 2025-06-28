using UnityEngine;


namespace Assets.Code.Gameplay.CameraSystem
{
    internal class SameraService
    {
        private readonly CameraMover _mover;
        private readonly CameraController _controller;

        public SameraService(CameraMover mover, CameraController controller)
        {
            _mover = mover;
            _controller = controller;
        }

        public void MoveToTarget(Transform target)
        {
            _mover.SetNewTarget(target);
        }
    }
}
