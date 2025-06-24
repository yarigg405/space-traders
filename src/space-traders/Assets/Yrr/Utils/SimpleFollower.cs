using UnityEngine;


namespace Yrr.Utils
{
    internal sealed class SimpleFollower : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        private Vector3 _offset;

        private void Start()
        {
            _offset = transform.position - _target.position;
        }

        private void Update()
        {
            transform.position = _target.transform.position + _offset;
        }
    }
}
