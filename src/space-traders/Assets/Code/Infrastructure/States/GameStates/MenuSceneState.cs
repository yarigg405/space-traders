using Assets.Code.Infrastructure.Loading;
using Assets.Code.Infrastructure.States.StateMachine;
using Assets.Code.Infrastructure.States.StatesInfrastructure;


namespace Assets.Code.Infrastructure.States.GameStates
{
    internal sealed class MenuSceneState : GameState
    {
        private readonly IStateMachine _stateMachine;
        private readonly IScenesLoader _scenesLoader;

        public MenuSceneState(IScenesLoader scenesLoader, IStateMachine stateMachine)
        {
            _scenesLoader = scenesLoader;
            _stateMachine = stateMachine;
        }

        public override void Enter()
        {
            _scenesLoader.LoadScene(SceneNames.MenuScene);
        }
    }
}
