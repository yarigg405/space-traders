using Assets.Code.ClientPart.Networking;
using Assets.Code.Infrastructure.Loading;
using Assets.Code.Infrastructure.States.StateMachine;
using Assets.Code.Infrastructure.States.StatesInfrastructure;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;


namespace Assets.Code.Infrastructure.States.GameStates
{
    internal sealed class ChangeGameSceneState : GamePayloadState<string>
    {
        private readonly IStateMachine _stateMachine;
        private readonly IScenesLoader _scenesLoader;

        public ChangeGameSceneState(IScenesLoader scenesLoader,
            IStateMachine stateMachine)
        {
            _scenesLoader = scenesLoader;
            _stateMachine = stateMachine;
        }

        public override void Enter(string sceneName)
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            EnterAsync(sceneName).Forget();
        }

        private async UniTask EnterAsync(string sceneName)
        {
            ClientMessenger.RequestForChangeScene(sceneName);

            sceneName = await ClientMessenger.RequestForConnectGame();
            _scenesLoader.LoadScene(sceneName);
        }

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            _stateMachine.Enter<GameLoopState>();
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}
