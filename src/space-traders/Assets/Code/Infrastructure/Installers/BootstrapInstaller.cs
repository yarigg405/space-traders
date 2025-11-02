using Assets.Code.Gameplay.Common.Time;
using Assets.Code.Gameplay.Features;
using Assets.Code.Infrastructure.AssetManagement;
using Assets.Code.Infrastructure.DI;
using Assets.Code.Infrastructure.EntryPoints;
using Assets.Code.Infrastructure.Loading;
using Assets.Code.Infrastructure.States.GameStates;
using Assets.Code.Infrastructure.States.StateMachine;
using Assets.Code.Infrastructure.Systems;
using Assets.Code.Networking;
using Assets.Code.Networking.ClientMaintenance;
using Assets.Code.Serialization.Services;
using Assets.Code.View.Factory;
using VContainer;
using VContainer.Unity;


namespace Assets.Code.Infrastructure.Installers
{
    internal sealed class BootstrapInstaller : MonoInstaller
    {
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
            Builder.Register<AssetProvider>(Lifetime.Singleton).AsImplementedInterfaces();
            Builder.Register<ScenesLoader>(Lifetime.Singleton).AsImplementedInterfaces();
            Builder.Register<EntityViewFactory>(Lifetime.Singleton).AsImplementedInterfaces();
            Builder.Register<GameStateMachine>(Lifetime.Singleton).AsImplementedInterfaces();
            Builder.Register<UnityTimeService>(Lifetime.Singleton).AsImplementedInterfaces();
            Builder.Register<WorldSerializationService>(Lifetime.Singleton).AsSelf();
            Builder.Register<FeaturesContainer>(Lifetime.Singleton).AsSelf();
        }

        private void BindNetworking()
        {
            Builder.Register<NetworkManager>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            Builder.Register<ClientMessengerDependencySetupper>(Lifetime.Singleton).AsImplementedInterfaces();
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
            Builder.Register<GameLoopState>(Lifetime.Transient).AsSelf();
        }

        private void RegisterEntryPoint()
        {
            Builder.RegisterEntryPoint<GameEntryPoint>();
        }
    }
}
