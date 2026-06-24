using Assets.Code.Infrastructure.Loading;
using Assets.Code.Infrastructure.States.StateMachine;
using Assets.Code.Infrastructure.States.StatesInfrastructure;
using Assets.Code.Networking.Data;


namespace Assets.Code.Infrastructure.States.GameStates
{
    internal sealed class LoadGameSceneState : GamePayloadState<SpaceSceneData>
    {
        private readonly IStateMachine _stateMachine;
        private readonly IScenesLoader _scenesLoader;
        private readonly SpaceSceneDataHolder _sceneDataHolder;


        public LoadGameSceneState(IStateMachine stateMachine,
            IScenesLoader scenesLoader,
            SpaceSceneDataHolder sceneDataHolder)
        {
            _stateMachine = stateMachine;
            _scenesLoader = scenesLoader;
            _sceneDataHolder = sceneDataHolder;
        }

        public override void Enter(SpaceSceneData sceneData)
        {
            _sceneDataHolder.Current = sceneData;
            _scenesLoader.LoadScene(sceneData.SceneName, OnSceneLoaded);
        }

        private void OnSceneLoaded()
        {
            _stateMachine.Enter<GameLoopState>();
            _scenesLoader.SetSceneLoaded();
        }
    }
}
