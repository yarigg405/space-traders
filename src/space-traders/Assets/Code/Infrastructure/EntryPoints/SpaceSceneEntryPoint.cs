using Assets.Code.Gameplay.Features;
using Assets.Code.Infrastructure.States.GameStates;
using Assets.Code.Infrastructure.States.StateMachine;
using Assets.Code.Infrastructure.Systems;
using Assets.Code.Networking.ClientMaintenance;
using Assets.Code.Serialization.Services;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer.Unity;


namespace Assets.Code.Infrastructure.EntryPoints
{
    internal sealed class SpaceSceneEntryPoint : IStartable
    {
        private readonly IStateMachine _stateMachine;
        private readonly ISystemFactory _systems;
        private readonly FeaturesContainer _featuresContainer;
        private readonly WorldSerializationService _worldSerialization;
        private readonly GameContext _gameContext;

        public SpaceSceneEntryPoint(IStateMachine stateMachine,
            ISystemFactory systems, FeaturesContainer featuresContainer,
            WorldSerializationService worldSerialization, GameContext gameContext)
        {
            _stateMachine = stateMachine;
            _systems = systems;
            _featuresContainer = featuresContainer;
            _worldSerialization = worldSerialization;
            _gameContext = gameContext;
        }

        void IStartable.Start()
        {
            var feature = _systems.Create<ClientGameFeature>();
            _featuresContainer.Cleanup();
            _featuresContainer.Add(feature);

            LoadEntitiesAsync().Forget();
        }

        private async UniTask LoadEntitiesAsync()
        {
            Debug.Log("### Fill context");
            var jsonForEntities = await ClientMessenger.RequestForLoadingSceneEntities();
            _worldSerialization.FillContext(jsonForEntities, _gameContext);
            Debug.Log("### Fill context");

            _featuresContainer.Initialize();
            _stateMachine.Enter<GameLoopState>();
        }            
    }
}
