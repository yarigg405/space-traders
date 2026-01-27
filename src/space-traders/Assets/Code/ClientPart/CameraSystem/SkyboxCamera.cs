using UnityEngine;


namespace Assets.Code.ClientPart.CameraSystem
{
    public sealed class SkyboxCamera : MonoBehaviour
    {
        private const float SKYBOX_OBJECTS_POSITION_MODIFIER = 0.0001f;

        [SerializeField] private Transform _skyboxCameraRoot;
        private Camera _mainCamera
        {
            get
            {
                if (!m_mainCamera)
                    m_mainCamera = Camera.main;
                return m_mainCamera;
            }
        }
        private Camera m_mainCamera;


        private void LateUpdate()
        {
            var pos = _mainCamera.transform.position * SKYBOX_OBJECTS_POSITION_MODIFIER;
            _skyboxCameraRoot.position = pos;

            var rotation = _mainCamera.transform.rotation;
            _skyboxCameraRoot.rotation = rotation;
        }
    }
}