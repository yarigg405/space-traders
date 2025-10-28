using Assets.Code.Infrastructure.Loading;
using Assets.Code.Infrastructure.States.StateMachine;
using Assets.Code.Infrastructure.States.StatesInfrastructure;
using Assets.Code.Networking.Messaging;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;


namespace Assets.Code.Infrastructure.States.GameStates
{
    internal sealed class LoadGameSceneState : GameState
    {
        private readonly IStateMachine _stateMachine;
        private readonly IScenesLoader _scenesLoader;


        public LoadGameSceneState(IStateMachine stateMachine,
            IScenesLoader scenesLoader)
        {
            _stateMachine = stateMachine;
            _scenesLoader = scenesLoader;
        }

        public override void Enter()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            EnterAsync().Forget();
        }

        private async UniTask EnterAsync()
        {
            var sceneName = await ClientMessenger.RequestForConnectGame();
            _scenesLoader.LoadScene(sceneName);
        }

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            _stateMachine.Enter<GameLoopState>();
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}
