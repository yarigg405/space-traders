using Assets.Code.Gameplay.CameraSystem;
using Assets.Code.Gameplay.Features.Player;
using Assets.Code.Gameplay.Features.Player.Factory;
using Assets.Code.Gameplay.InputInteraction;
using Assets.Code.Infrastructure.DI;
using Assets.Code.Infrastructure.Loading;
using Assets.Code.Infrastructure.Systems;
using UnityEngine;
using VContainer;
using VContainer.Unity;


namespace Assets.Code.Infrastructure.Installers
{
    internal sealed class GameSceneInstaller : MonoInstaller
    {
        [SerializeField] private CameraMover _cameraMover;
        [SerializeField] private CameraController _cameraController;

        private IContainerBuilder _builder;

        public override void Install(IContainerBuilder builder)
        {
            _builder = builder;

            RegisterInfrastructure();
            RegisterFactories();
            RegisterInput();

            _builder.RegisterEntryPoint<SpaceSceneEntryPoint>();
        }

        private void RegisterInfrastructure()
        {
            _builder.Register<PlayerProvider>(Lifetime.Scoped).AsSelf().AsImplementedInterfaces();
            var cameraService = new CameraService(_cameraMover, _cameraController);
            _builder.RegisterInstance(cameraService);
            _builder.Register<PlayerBindService>(Lifetime.Scoped).AsImplementedInterfaces();
        }

        private void RegisterFactories()
        {
            _builder.Register<SystemFactory>(Lifetime.Scoped).AsImplementedInterfaces();
            _builder.Register<PlayerFactory>(Lifetime.Scoped).AsSelf();
        }

        private void RegisterInput()
        {
            _builder.Register<CameraRaycaster>(Lifetime.Scoped).AsSelf();
            _builder.Register<MouseClickNotificator>(Lifetime.Scoped).AsSelf().AsImplementedInterfaces();
            _builder.Register<InputService>(Lifetime.Scoped).AsImplementedInterfaces();
        }
    }
}
