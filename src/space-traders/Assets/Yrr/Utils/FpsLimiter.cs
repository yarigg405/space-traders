using UnityEngine;


namespace Yrr.Utils
{
    internal sealed class FpsLimiter : MonoBehaviour
    {
        [SerializeField] private int targetFps = 60;

        private void Start()
        {
            Application.targetFrameRate = targetFps;
        }
    }
}
