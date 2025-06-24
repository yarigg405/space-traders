using UnityEngine;


namespace Yrr.Utils
{
    internal sealed class SmoothFollower : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private float _smoothModificator = 1;
        private Vector3 _offset;

        private void Start()
        {
            _offset = transform.position - _target.position;
        }

        private void FixedUpdate()
        {
            var point = _target.transform.position + _offset;
            transform.position = Vector3.MoveTowards(transform.position, point, _smoothModificator * Time.fixedDeltaTime * 100f);
        }
    }
}