using Unity.Cinemachine;
using UnityEngine;


namespace Assets.Code.ClientPart.CameraSystem
{
    public sealed class CameraTargetController : MonoBehaviour
    {
        [SerializeField] private CinemachineCamera _vCam;
        [SerializeField] private CinemachineBrain _brain;

        private Transform _currentTarget;

        internal void ManualUpdate()
        {
            if (_currentTarget == null) return;

            transform.position = _currentTarget.position;
            _brain.ManualUpdate();
        }

        internal void SetNewTarget(Transform newTarget)
        {
            if (_currentTarget == newTarget) return;

            _currentTarget = newTarget;
            transform.position = _currentTarget.position;
        }
    }
}
