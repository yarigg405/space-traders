using Assets.Code.Infrastructure.Loading;
using Assets.Code.Infrastructure.States.StateMachine;
using Assets.Code.Infrastructure.States.StatesInfrastructure;
using UnityEngine.SceneManagement;


namespace Assets.Code.Infrastructure.States.GameStates
{
    internal sealed class LoadBattleState : GamePayloadState<string>
    {
        private readonly IStateMachine _stateMachine;
        private readonly IScenesLoader _scenesLoader;

        public LoadBattleState(IStateMachine stateMachine, IScenesLoader scenesLoader)
        {
            _stateMachine = stateMachine;
            _scenesLoader = scenesLoader;
        }

        public override void Enter(string sceneName)
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            _scenesLoader.LoadScene(sceneName);
        }

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            _stateMachine.Enter<GameLoopState>();
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}
