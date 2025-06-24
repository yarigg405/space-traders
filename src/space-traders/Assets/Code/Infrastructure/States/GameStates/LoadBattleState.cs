using Assets.Code.Infrastructure.States.StateMachine;
using Assets.Code.Infrastructure.States.StatesInfrastructure;
using UnityEngine.SceneManagement;


namespace Assets.Code.Infrastructure.States.GameStates
{
    internal sealed class LoadBattleState : GamePayloadState<string>
    {
        private readonly IStateMachine _stateMachine;

        public LoadBattleState(IStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public override void Enter(string sceneName)
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            _stateMachine.Enter<GameLoopState>();
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}
