using Assets.Code.ClientPart.Networking;
using Assets.Code.Infrastructure.Loading;
using Assets.Code.Infrastructure.States.StateMachine;
using Assets.Code.Infrastructure.States.StatesInfrastructure;
using Assets.Code.Networking;
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
        private readonly AuthentificationContainer _authentification;

        private CancellationTokenSource _cts;

        public ChangeGameSceneState(IScenesLoader scenesLoader,
            IStateMachine stateMachine,
            ClientMessenger messenger,
            AuthentificationContainer authentification)
        {
            _scenesLoader = scenesLoader;
            _stateMachine = stateMachine;
            _messenger = messenger;
            _authentification = authentification;
        }

        public override void Enter(string sceneName)
        {
            _cts = new();
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

            sceneName = await _messenger.RequestForEnterTheGame(_authentification.SelectedCharacterId, _cts.Token);
            _scenesLoader.LoadScene(sceneName, OnSceneLoaded);
        }

        private void OnSceneLoaded()
        {
            _scenesLoader.SetSceneLoaded();
            _stateMachine.Enter<GameLoopState>();
        }
    }
}
