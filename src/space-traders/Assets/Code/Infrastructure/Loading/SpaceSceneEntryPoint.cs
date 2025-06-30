using Assets.Code.Gameplay;
using Assets.Code.Infrastructure.States.GameStates;
using Assets.Code.Infrastructure.States.StateMachine;
using Assets.Code.Infrastructure.Systems;
using VContainer.Unity;


namespace Assets.Code.Infrastructure.Loading
{
    internal sealed class SpaceSceneEntryPoint : IStartable
    {
        private readonly IStateMachine _stateMachine;
        private readonly ISystemFactory _systems;
        private readonly FeaturesContainer _featuresContainer;

        public SpaceSceneEntryPoint(IStateMachine stateMachine, ISystemFactory systems, FeaturesContainer featuresContainer)
        {
            _stateMachine = stateMachine;
            _systems = systems;
            _featuresContainer = featuresContainer;
        }

        void IStartable.Start()
        {
            _stateMachine.Enter<GameLoopState>();

            var feature = _systems.Create<GameFeature>();
            _featuresContainer.Add(feature);
            _featuresContainer.Initialize();
        }
    }
}
