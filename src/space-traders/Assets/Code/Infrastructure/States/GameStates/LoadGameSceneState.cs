using Assets.Code.Infrastructure.States.StateMachine;
using Assets.Code.Infrastructure.States.StatesInfrastructure;
using UnityEngine.SceneManagement;
using Assets.Code.Networking;


namespace Assets.Code.Infrastructure.States.GameStates
{
    internal sealed class LoadGameSceneState : GamePayloadState<string>
    {
        private readonly IStateMachine _stateMachine;
        private readonly NetworkManager _networkManager;

        public LoadGameSceneState(IStateMachine stateMachine, NetworkManager networkManager)
        {
            _stateMachine = stateMachine;
            _networkManager = networkManager;
        }

        public override void Enter(string sceneName)
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            _networkManager.DisconnectCurrentScene();
            _networkManager.RequestConnectToScene(sceneName);  
        }

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            _stateMachine.Enter<GameLoopState>();
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}
