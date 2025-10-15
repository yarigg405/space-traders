using Assets.Code.Infrastructure.Loading;
using Assets.Code.Infrastructure.States.GameStates;
using Assets.Code.Infrastructure.States.StateMachine;
using Assets.Code.Networking;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using VContainer;


namespace Assets.Code.UI
{
    public sealed class MainMenuScreen : MonoBehaviour
    {
        [Inject] private readonly NetworkManager _networkManager;
        [Inject] private readonly IStateMachine _stateMachine;

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
            OnHostClickAsync().Forget();
        }

        private void OnClientClick()
        {
            OnClientClickedAsync().Forget();
        }

        private async UniTask OnHostClickAsync()
        {
            await _networkManager.StartHost();
            _stateMachine.Enter<LoadGameSceneState, string>(SceneNames.GameScene1);
        }

        private async UniTask OnClientClickedAsync()
        {
            await _networkManager.StartClient();
            _stateMachine.Enter<LoadGameSceneState, string>(SceneNames.GameScene1);
        }
    }
}