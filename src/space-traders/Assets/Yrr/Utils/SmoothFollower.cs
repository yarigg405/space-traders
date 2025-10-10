using UnityEngine;


namespace Yrr.Utils
{
    internal sealed class SmoothFollower : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private float smoothModificator = 1;
        private Vector3 _offset;

        private void Start()
        {
            _offset = transform.position - target.position;
        }

        private void FixedUpdate()
        {
            var point = target.transform.position + _offset;
            transform.position = Vector3.MoveTowards(transform.position, point, smoothModificator * Time.fixedDeltaTime * 100f);
        }
    }
}