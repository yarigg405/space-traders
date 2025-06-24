using System.Collections;
using UnityEngine;


namespace Yrr.Utils
{
    public sealed class SpeedReckoner : MonoBehaviour
    {
        [SerializeField] private float _updateDelay = 0.05f;
        public ReactiveValue<float> RealSpeed = new();
        public ReactiveValue<Vector3> RealDirection = new();


        private void OnEnable()
        {
            StartCoroutine(ReckonerCoroutine());
        }


        private IEnumerator ReckonerCoroutine()
        {
            var timedWait = new WaitForSeconds(_updateDelay);
            var lastPosition = transform.position;
            float lastTimestamp = Time.time;

            while (enabled)
            {
                yield return timedWait;

                var deltaPosition = transform.position - lastPosition;
                RealDirection.Value = deltaPosition;

                var deltaLenght = deltaPosition.magnitude;

                var time = Time.time;
                var deltaTime = time - lastTimestamp;


                if (Mathf.Approximately(deltaLenght, 0f)) // Clean up "near-zero" displacement
                    deltaLenght = 0f;

                RealSpeed.Value = deltaLenght / deltaTime;
                lastPosition = transform.position;
                lastTimestamp = time;
            }
        }
    }
}
