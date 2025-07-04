using Assets.Code.Gameplay.CameraSystem;
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
            RegisterInput();

            _builder.RegisterEntryPoint<SpaceSceneEntryPoint>();
        }

        private void RegisterInfrastructure()
        {
            _builder.Register<SystemFactory>(Lifetime.Singleton).AsImplementedInterfaces();
            var cameraService = new CameraService(_cameraMover, _cameraController);
            _builder.RegisterInstance(cameraService);
        }

        private void RegisterInput()
        {
            _builder.Register<CameraRaycaster>(Lifetime.Scoped).AsSelf();
            _builder.Register<MouseClickNotificator>(Lifetime.Scoped).AsSelf().AsImplementedInterfaces();
            _builder.Register<InputService>(Lifetime.Scoped).AsImplementedInterfaces();
        }
    }
}
