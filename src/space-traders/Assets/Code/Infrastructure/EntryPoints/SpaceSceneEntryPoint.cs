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
        private readonly ClientMessenger _messenger;

        public SpaceSceneEntryPoint(ISystemFactory systems,
            FeaturesContainer featuresContainer,
            ClientMessenger messenger)
        {
            _systems = systems;
            _featuresContainer = featuresContainer;
            _messenger = messenger;
        }

        void IStartable.Start()
        {
            var feature = _systems.Create<ClientGameFeature>();
            _featuresContainer.Cleanup();
            _featuresContainer.Add(feature);
            _featuresContainer.Initialize();
            _messenger.RequestForLoadingSceneEntities();
        }
    }
}
