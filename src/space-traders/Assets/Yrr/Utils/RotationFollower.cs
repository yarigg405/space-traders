using UnityEngine;


namespace Yrr.Utils
{
    internal sealed class RotationFollower : MonoBehaviour
    {
        [SerializeField] private Transform _target;

        private void Update()
        {
            transform.rotation = _target.rotation;
        }
    }
}