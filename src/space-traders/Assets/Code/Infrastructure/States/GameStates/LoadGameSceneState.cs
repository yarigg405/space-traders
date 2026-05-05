using Assets.Code.Infrastructure.Loading;
using Assets.Code.Infrastructure.States.StateMachine;
using Assets.Code.Infrastructure.States.StatesInfrastructure;
using UnityEngine.SceneManagement;


namespace Assets.Code.Infrastructure.States.GameStates
{
    internal sealed class LoadGameSceneState : GamePayloadState<string>
    {
        private readonly IStateMachine _stateMachine;
        private readonly IScenesLoader _scenesLoader;


        public LoadGameSceneState(IStateMachine stateMachine,
            IScenesLoader scenesLoader)
        {
            _stateMachine = stateMachine;
            _scenesLoader = scenesLoader;
        }

        public override void Enter(string sceneName)
        {
            _scenesLoader.LoadScene(sceneName, OnSceneLoaded);
        }

        private void OnSceneLoaded()
        {
            _scenesLoader.SetSceneLoaded();
            _stateMachine.Enter<GameLoopState>();
        }
    }
}
