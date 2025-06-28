using Assets.Code.Gameplay.Common.Collisions;
using Assets.Code.Gameplay.Common.Time;
using Assets.Code.Infrastructure.DI;
using Assets.Code.Infrastructure.Identifiers;
using Assets.Code.Infrastructure.Loading;
using Assets.Code.Infrastructure.States.GameStates;
using Assets.Code.Infrastructure.States.StateMachine;
using Assets.Code.Infrastructure.Systems;
using VContainer;
using VContainer.Unity;


namespace Assets.Code.Infrastructure.Installers
{
    public sealed class BootstrapInstaller : MonoInstaller
    {
        private IContainerBuilder _builder;

        public override void Install(IContainerBuilder builder)
        {
            _builder = builder;
            BindContexts();
            BindInfrastructureServices();
            BindStates();
            BindCommonServices();

            RegisterEntryPoint();
        }

        private void BindContexts()
        {
            _builder.RegisterInstance(Contexts.sharedInstance).AsSelf();
            _builder.RegisterInstance(Contexts.sharedInstance.game).AsSelf();
            _builder.RegisterInstance(Contexts.sharedInstance.input).AsSelf();
            _builder.RegisterInstance(Contexts.sharedInstance.meta).AsSelf();
        }

        private void BindInfrastructureServices()
        {
            _builder.Register<IdentifierService>(Lifetime.Singleton).AsSelf();
            _builder.Register<GameStateMachine>(Lifetime.Singleton).AsImplementedInterfaces();
            _builder.Register<ScenesLoader>(Lifetime.Singleton).AsImplementedInterfaces();
            _builder.Register<SystemFactory>(Lifetime.Singleton).AsImplementedInterfaces();
        }

        private void BindStates()
        {
            _builder.Register<BootstrapState>(Lifetime.Transient).AsSelf();
            _builder.Register<LoadHomeScreenState>(Lifetime.Transient).AsSelf();
            _builder.Register<MenuSceneState>(Lifetime.Transient).AsSelf();
            _builder.Register<LoadBattleState>(Lifetime.Transient).AsSelf();
            _builder.Register<GameLoopState>(Lifetime.Transient).AsSelf();
        }

        private void BindCommonServices()
        {
            _builder.Register<UnityTimeService>(Lifetime.Singleton).AsImplementedInterfaces();
            _builder.Register<CollisionRegistry>(Lifetime.Singleton).AsImplementedInterfaces();
        }

        private void RegisterEntryPoint()
        {
            _builder.RegisterEntryPoint<GameEntryPoint>();
        }
    }
}
