using Assets.Code.Gameplay.CameraSystem;
using Assets.Code.Gameplay.InputInteraction;
using Assets.Code.Infrastructure.DI;
using UnityEngine;
using VContainer;


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

            RegisterCamera();
            RegisterInput();
        }

        private void RegisterCamera()
        {
            var cameraService = new SameraService(_cameraMover, _cameraController);
            _builder.RegisterInstance(cameraService);
        }

        private void RegisterInput()
        {
            _builder.Register<CameraRaycaster>(Lifetime.Scoped).AsSelf();
            _builder.Register<MouseClickNotificator>(Lifetime.Scoped).AsSelf().AsImplementedInterfaces();
            _builder.Register<InputListener>(Lifetime.Scoped).AsSelf().AsImplementedInterfaces();
        }
    }
}
