using Assets.Code.Gameplay.Common.Time;
using Assets.Code.Gameplay.Worlds;
using Assets.Code.Infrastructure.AssetManagement;
using Assets.Code.Infrastructure.DI;
using Assets.Code.Infrastructure.EntryPoints;
using Assets.Code.Infrastructure.Identifiers;
using Assets.Code.Infrastructure.Loading;
using Assets.Code.Infrastructure.States.GameStates;
using Assets.Code.Infrastructure.States.StateMachine;
using Assets.Code.Networking;
using Assets.Code.Networking.Messaging;
using Assets.Code.View.Factory;
using VContainer;
using VContainer.Unity;


namespace Assets.Code.Infrastructure.Installers
{
    internal sealed class BootstrapInstaller : MonoInstaller
    {
        private IContainerBuilder _builder;

        public override void Install(IContainerBuilder builder)
        {
            _builder = builder;

            BindGameServices();
            BindNetworking();
            BindStates();

            RegisterEntryPoint();
        }

        private void BindGameServices()
        {
            _builder.Register<AssetProvider>(Lifetime.Singleton).AsImplementedInterfaces();
            _builder.Register<ScenesLoader>(Lifetime.Singleton).AsImplementedInterfaces();
            _builder.Register<EntityViewFactory>(Lifetime.Singleton).AsImplementedInterfaces();
            _builder.Register<IdentifierService>(Lifetime.Singleton).AsImplementedInterfaces();
            _builder.Register<GameStateMachine>(Lifetime.Singleton).AsImplementedInterfaces();
            _builder.Register<UnityTimeService>(Lifetime.Singleton).AsImplementedInterfaces();
        }

        private void BindNetworking()
        {
            _builder.Register<NetworkManager>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            _builder.Register<NetworkDependencySetupper>(Lifetime.Singleton).AsImplementedInterfaces();
            _builder.Register<ClientsScenesContainer>(Lifetime.Singleton).AsSelf();
            _builder.Register<EcsWorldsBuilder>(Lifetime.Singleton).AsSelf();
            _builder.Register<ServerWorldsController>(Lifetime.Singleton).AsImplementedInterfaces();
        }

        private void BindStates()
        {
            _builder.Register<BootstrapState>(Lifetime.Transient).AsSelf();
            _builder.Register<MenuSceneState>(Lifetime.Transient).AsSelf();
            _builder.Register<LoadGameSceneState>(Lifetime.Transient).AsSelf();
            _builder.Register<GameLoopState>(Lifetime.Transient).AsSelf();
        }

        private void RegisterEntryPoint()
        {
            _builder.RegisterEntryPoint<GameEntryPoint>();
        }
    }
}
