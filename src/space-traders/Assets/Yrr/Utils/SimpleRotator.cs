using UnityEngine;


namespace Yrr.Utils
{
    internal sealed class SimpleRotator : MonoBehaviour
    {
        [SerializeField] private Vector3 _rotation;

        private void Update()
        {
            transform.Rotate(_rotation * Time.deltaTime);
        }
    }
}