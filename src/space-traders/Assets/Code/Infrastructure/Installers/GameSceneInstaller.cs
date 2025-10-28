using Assets.Code.Gameplay.Common.CameraSystem;
using Assets.Code.Gameplay.Features.Player.Factory;
using Assets.Code.Gameplay.Features.Player.Infrastructure;
using Assets.Code.Gameplay.Worlds;
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

            RegisterCommonServices();
            RegisterPlayerServices();
            RegisterFactories();

            _builder.RegisterEntryPoint<SpaceSceneEntryPoint>();
        }

        private void RegisterCommonServices()
        {
            _builder.Register<CameraService>(Lifetime.Scoped).AsSelf();
        }

        private void RegisterPlayerServices()
        {
            _builder.Register<PlayerProvider>(Lifetime.Scoped).AsImplementedInterfaces();
        }

        private void RegisterFactories()
        {
            _builder.Register<PlayerFactory>(Lifetime.Scoped).AsSelf();
        }
    }
}
