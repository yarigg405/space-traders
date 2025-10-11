using Assets.Code._Unrefactored.Network;
using Assets.Code.Infrastructure.Loading;
using UnityEngine;
using UnityEngine.UI;
using VContainer;


namespace Assets.Code.UI
{
    public sealed class MainMenuScreen : MonoBehaviour
    {
        [Inject] private readonly IScenesLoader _scenesLoader;
        [Inject] private readonly NetworkManager _networkManager;

        [SerializeField] private Button _hostButton;
        [SerializeField] private Button _clientButton;

        private void OnEnable()
        {
            _hostButton.onClick.AddListener(OnHostClick);
            _clientButton.onClick.AddListener(OnClientClick);
        }

        private void OnDisable()
        {
            _hostButton.onClick.RemoveListener(OnHostClick);
            _clientButton.onClick.RemoveListener(OnClientClick);
        }

        private void OnHostClick()
        {
            _networkManager.StartHost();
        }

        private void OnClientClick()
        {
            _networkManager.StartClient();
        }
    }
}