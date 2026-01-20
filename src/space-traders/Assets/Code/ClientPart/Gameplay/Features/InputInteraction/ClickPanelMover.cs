using UnityEngine;


namespace Yrr.Utils
{
    internal sealed class ClickPanelMover : MonoBehaviour
    {
        private Camera _mainCamera;

        private void Start()
        {
            _mainCamera = Camera.main;
        }

        private void LateUpdate()
        {
            var trPos = _mainCamera.transform.position;
            trPos.y = 0;
            transform.position = trPos;
        }
    }
}