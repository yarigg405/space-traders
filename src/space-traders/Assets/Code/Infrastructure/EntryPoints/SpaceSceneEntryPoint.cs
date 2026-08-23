using Assets.Code.ClientPart.Gameplay.Features;
using Assets.Code.ClientPart.Networking;
using Assets.Code.Infrastructure.Systems;
using Assets.Code.UI;
using Assets.Code.UI.Screens.GameMain;
using Assets.Code.UI.Screens.GameSceneScreens.GameSceneMain;
using VContainer.Unity;


namespace Assets.Code.Infrastructure.EntryPoints
{
    internal sealed class SpaceSceneEntryPoint : IStartable
    {
        private readonly ISystemFactory _systems;
        private readonly FeaturesContainer _featuresContainer;
        private readonly ClientMessenger _messenger;
        private readonly IUIManager _uiManager;

        public SpaceSceneEntryPoint(ISystemFactory systems,
            FeaturesContainer featuresContainer,
            ClientMessenger messenger,
            IUIManager uiManager)
        {
            _systems = systems;
            _featuresContainer = featuresContainer;
            _messenger = messenger;
            _uiManager = uiManager;
        }

        void IStartable.Start()
        {
            var feature = _systems.Create<ClientGameFeature>();
            _featuresContainer.Cleanup();
            _featuresContainer.Add(feature);
            _featuresContainer.Initialize();
            _messenger.RequestForLoadingSceneEntities();

            _uiManager.GoToScreen<GameSceneMainScreen>();
            _uiManager.ClearHistory();
            _uiManager.OpenModal<GameMainScreen>();
        }
    }
}
