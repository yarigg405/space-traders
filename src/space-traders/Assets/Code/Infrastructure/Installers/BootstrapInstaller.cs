using Assets.Code.Gameplay;
using Assets.Code.Gameplay.Common.Physics;
using Assets.Code.Gameplay.Common.Time;
using Assets.Code.Infrastructure.AssetManagement;
using Assets.Code.Infrastructure.DI;
using Assets.Code.Infrastructure.EntryPoints;
using Assets.Code.Infrastructure.Identifiers;
using Assets.Code.Infrastructure.Loading;
using Assets.Code.Infrastructure.States.GameStates;
using Assets.Code.Infrastructure.States.StateMachine;
using Assets.Code.Networking;
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
            BindContexts();
            BindGameServices();
            BindNetworking();
            BindStates();

            RegisterEntryPoint();
        }

        private void BindContexts()
        {
            _builder.RegisterInstance(Contexts.sharedInstance).AsSelf();
            _builder.RegisterInstance(Contexts.sharedInstance.game).AsSelf();
            _builder.RegisterInstance(Contexts.sharedInstance.input).AsSelf();
            _builder.RegisterInstance(Contexts.sharedInstance.meta).AsSelf();
        }

        private void BindGameServices()
        {
            _builder.Register<AssetProvider>(Lifetime.Singleton).AsImplementedInterfaces();
            _builder.Register<EntityViewFactory>(Lifetime.Singleton).AsImplementedInterfaces();
            _builder.Register<IdentifierService>(Lifetime.Singleton).AsImplementedInterfaces();
            _builder.Register<GameStateMachine>(Lifetime.Singleton).AsImplementedInterfaces();
            _builder.Register<ScenesLoader>(Lifetime.Singleton).AsImplementedInterfaces(); 
            _builder.Register<UnityTimeService>(Lifetime.Singleton).AsImplementedInterfaces();
            _builder.Register<CollisionRegistry>(Lifetime.Singleton).AsImplementedInterfaces();
        }

        private void BindNetworking()
        {
            _builder.Register<NetworkManager>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
        }

        private void BindStates()
        {
            _builder.Register<BootstrapState>(Lifetime.Transient).AsSelf();
            _builder.Register<MenuSceneState>(Lifetime.Transient).AsSelf();
            _builder.Register<LoadGameSceneState>(Lifetime.Transient).AsSelf();
            _builder.Register<GameLoopState>(Lifetime.Transient).AsSelf();
            _builder.Register<FeaturesContainer>(Lifetime.Singleton).AsSelf();
        }

        private void RegisterEntryPoint()
        {
            _builder.RegisterEntryPoint<GameEntryPoint>();
        }
    }
}
