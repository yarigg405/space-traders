using Assets.Code.ClientPart.Gameplay.Features;
using Assets.Code.ClientPart.Networking;
using Assets.Code.Infrastructure.Systems;
using VContainer.Unity;


namespace Assets.Code.Infrastructure.EntryPoints
{
    internal sealed class SpaceSceneEntryPoint : IStartable
    {
        private readonly ISystemFactory _systems;
        private readonly FeaturesContainer _featuresContainer;

        public SpaceSceneEntryPoint(ISystemFactory systems, 
            FeaturesContainer featuresContainer)
        {
            _systems = systems;
            _featuresContainer = featuresContainer;
        }

        void IStartable.Start()
        {
            var feature = _systems.Create<ClientGameFeature>();
            _featuresContainer.Cleanup();
            _featuresContainer.Add(feature);
            _featuresContainer.Initialize();
            ClientMessenger.RequestForLoadingSceneEntities();
        }
    }
}
