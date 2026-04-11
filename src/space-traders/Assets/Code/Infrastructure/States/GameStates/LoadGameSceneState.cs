using Assets.Code.ClientPart.Networking;
using Assets.Code.Infrastructure.Loading;
using Assets.Code.Infrastructure.States.StateMachine;
using Assets.Code.Infrastructure.States.StatesInfrastructure;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine.SceneManagement;


namespace Assets.Code.Infrastructure.States.GameStates
{
    internal sealed class LoadGameSceneState : GameState
    {
        private readonly IStateMachine _stateMachine;
        private readonly IScenesLoader _scenesLoader;
        private readonly ClientMessenger _messenger;

        private CancellationTokenSource _cts;


        public LoadGameSceneState(IStateMachine stateMachine,
            IScenesLoader scenesLoader,
            ClientMessenger messenger)
        {
            _stateMachine = stateMachine;
            _scenesLoader = scenesLoader;
            _messenger = messenger;
        }

        public override void Enter()
        {
            _cts = new();
            SceneManager.sceneLoaded += OnSceneLoaded;
            EnterAsync().Forget();
        }

        public override void Exit()
        {
            _cts?.Cancel();
            _cts?.Dispose();
        }

        private async UniTask EnterAsync()
        {
            var sceneName = await _messenger.RequestForEnterTheGame(_cts.Token);
            _scenesLoader.LoadScene(sceneName);
        }

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            _stateMachine.Enter<GameLoopState>();
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}
