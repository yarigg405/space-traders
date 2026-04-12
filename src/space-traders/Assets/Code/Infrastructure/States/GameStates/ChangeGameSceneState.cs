using Assets.Code.ClientPart.Networking;
using Assets.Code.Infrastructure.Loading;
using Assets.Code.Infrastructure.States.StateMachine;
using Assets.Code.Infrastructure.States.StatesInfrastructure;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine.SceneManagement;


namespace Assets.Code.Infrastructure.States.GameStates
{
    internal sealed class ChangeGameSceneState : GamePayloadState<string>
    {
        private readonly IStateMachine _stateMachine;
        private readonly IScenesLoader _scenesLoader;
        private readonly ClientMessenger _messenger;

        private CancellationTokenSource _cts;

        public ChangeGameSceneState(IScenesLoader scenesLoader,
            IStateMachine stateMachine,
            ClientMessenger messenger)
        {
            _scenesLoader = scenesLoader;
            _stateMachine = stateMachine;
            _messenger = messenger;
        }

        public override void Enter(string sceneName)
        {
            _cts = new();
            SceneManager.sceneLoaded += OnSceneLoaded;
            EnterAsync(sceneName).Forget();
        }

        public override void Exit()
        {
            _cts?.Cancel();
            _cts?.Dispose();
        }

        private async UniTask EnterAsync(string sceneName)
        {
            _messenger.RequestForChangeScene(sceneName);

            sceneName = await _messenger.RequestForEnterTheGame(_cts.Token);
            _scenesLoader.LoadScene(sceneName);
        }

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            _stateMachine.Enter<GameLoopState>();
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}
