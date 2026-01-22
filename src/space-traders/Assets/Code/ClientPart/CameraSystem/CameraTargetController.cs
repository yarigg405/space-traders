using Unity.Cinemachine;
using UnityEngine;


namespace Assets.Code.ClientPart.CameraSystem
{
    public sealed class CameraTargetController : MonoBehaviour
    {
        [SerializeField] private CinemachineCamera _vCam;

        private Transform _currentTarget;

        internal void SetNewTarget(Transform newTarget)
        {
            if (_currentTarget == newTarget) return;

            _currentTarget = newTarget;
            _vCam.LookAt = newTarget;
            _vCam.Follow = _currentTarget;
        }
    }
}
