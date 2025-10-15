using Assets.Code.Gameplay.Common.CameraSystem;
using Assets.Code.Gameplay.Features.Player.Factory;
using Assets.Code.Infrastructure.DI;
using Assets.Code.Infrastructure.EntryPoints;
using Assets.Code.Infrastructure.Systems;
using VContainer;
using VContainer.Unity;


namespace Assets.Code.Infrastructure.Installers
{
    internal sealed class GameSceneInstaller : MonoInstaller
    {
        private IContainerBuilder _builder;
        public override void Install(IContainerBuilder builder)
        {
            _builder = builder;

            RegisterInfrastructure();
            RegisterFactories();

            _builder.RegisterEntryPoint<SpaceSceneEntryPoint>();
        }

        private void RegisterInfrastructure()
        {
            _builder.Register<CameraService>(Lifetime.Scoped).AsSelf();
        }

        private void RegisterFactories()
        {
            _builder.Register<SystemFactory>(Lifetime.Scoped).AsImplementedInterfaces();
            _builder.Register<PlayerFactory>(Lifetime.Scoped).AsSelf();
        }
    }
}
