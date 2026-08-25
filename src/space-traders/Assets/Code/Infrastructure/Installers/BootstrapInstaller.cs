using Assets.Code.ClientPart;
using Assets.Code.ClientPart.AssetManagement;
using Assets.Code.ClientPart.Gameplay.Features;
using Assets.Code.ClientPart.Gameplay.Features.InputInteraction;
using Assets.Code.ClientPart.Gameplay.Features.Player.Services;
using Assets.Code.ClientPart.Networking;
using Assets.Code.Common.StaticData;
using Assets.Code.Common.Time;
using Assets.Code.Common.TradingSystem;
using Assets.Code.Infrastructure.DI;
using Assets.Code.Infrastructure.EntryPoints;
using Assets.Code.Infrastructure.Loading;
using Assets.Code.Infrastructure.States.GameStates;
using Assets.Code.Infrastructure.States.StateMachine;
using Assets.Code.Infrastructure.Systems;
using Assets.Code.Networking;
using Assets.Code.ServerPart.Physics.Data;
using UnityEngine;
using VContainer;
using VContainer.Unity;


namespace Assets.Code.Infrastructure.Installers
{
    internal sealed class BootstrapInstaller : MonoInstaller
    {
        [SerializeField] private ConfigsStorage _configsStorage;
        [SerializeField] private PhysicsShapesStorage _shapesStorage;
        [SerializeField] private ItemsCatalogSO _itemsCatalog;
        [SerializeField] private TradeItemsCategoryConfig _categoriesConfig;
        [SerializeField] private TradeOrdersGenerationConfig _ordersGenerationConfig;

        protected override void Install()
        {
            BindContexts();
            BindGameServices();
            BindStates();
            BindNetworking();
            BindFactories();

            RegisterEntryPoint();
        }
        private void BindContexts()
        {
            Builder.RegisterInstance(Contexts.sharedInstance).AsSelf();
            Builder.RegisterInstance(Contexts.sharedInstance.game).AsSelf();
            Builder.RegisterInstance(Contexts.sharedInstance.input).AsSelf();
            Builder.RegisterInstance(Contexts.sharedInstance.meta).AsSelf();
        }

        private void BindGameServices()
        {
            Builder.Register<LifetimeCancellationToken>(Lifetime.Singleton).AsImplementedInterfaces();
            Builder.Register<AssetProvider>(Lifetime.Singleton).AsImplementedInterfaces();
            Builder.Register<ScenesLoader>(Lifetime.Singleton).AsImplementedInterfaces();
            Builder.Register<SpaceSceneDataHolder>(Lifetime.Singleton).AsSelf();
            Builder.Register<StationSceneDataHolder>(Lifetime.Singleton).AsSelf();
            Builder.Register<GameStateMachine>(Lifetime.Singleton).AsImplementedInterfaces();
            Builder.Register<UnityTimeService>(Lifetime.Singleton).AsImplementedInterfaces();
            Builder.Register<FeaturesContainer>(Lifetime.Singleton).AsSelf();
            Builder.Register<ClientEntitiesController>(Lifetime.Singleton).AsSelf();
            Builder.Register<InputReferencesContainer>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            Builder.Register<ClientDockingService>(Lifetime.Scoped).AsSelf();

            Builder.RegisterInstance(_configsStorage);
            Builder.RegisterInstance(_shapesStorage).AsImplementedInterfaces();
            Builder.RegisterInstance(_itemsCatalog).AsImplementedInterfaces();
            Builder.RegisterInstance(_categoriesConfig);
            Builder.RegisterInstance(_ordersGenerationConfig);
        }

        private void BindNetworking()
        {
            Builder.Register<AuthenticationContainer>(Lifetime.Singleton).AsSelf();
            Builder.Register<GameEnterManager>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            Builder.Register<NetworkManager>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            Builder.Register<ClientMessenger>(Lifetime.Singleton).AsSelf();
            Builder.Register<NetworkRequestSystem>(Lifetime.Singleton).AsSelf();
            Builder.Register<ClientMessengerRouter>(Lifetime.Singleton).AsSelf();
            Builder.Register<ClientMessengerDependencySetupper>(Lifetime.Singleton).AsImplementedInterfaces();
            Builder.Register<TickCounter>(Lifetime.Singleton);
            Builder.Register<InterpolationClock>(Lifetime.Singleton);
            Builder.Register<ClockSyncService>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            Builder.Register<ClientCommandBuffer>(Lifetime.Singleton).AsSelf();
        }

        private void BindFactories()
        {
            Builder.Register<SystemFactory>(Lifetime.Scoped).AsImplementedInterfaces();
        }

        private void BindStates()
        {
            Builder.Register<BootstrapState>(Lifetime.Transient).AsSelf();
            Builder.Register<MenuSceneState>(Lifetime.Transient).AsSelf();
            Builder.Register<LoadGameSceneState>(Lifetime.Transient).AsSelf();
            Builder.Register<SpaceStationState>(Lifetime.Transient).AsSelf();
            Builder.Register<GameLoopState>(Lifetime.Transient).AsSelf().AsImplementedInterfaces();
        }

        private void RegisterEntryPoint()
        {
            Builder.RegisterEntryPoint<GameEntryPoint>();
        }
    }
}
