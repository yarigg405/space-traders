using Assets.Code.ClientPart.CameraSystem;
using Assets.Code.ClientPart.Gameplay.Features.InputInteraction;
using Assets.Code.ClientPart.Gameplay.Features.Player.Infrastructure;
using Assets.Code.Infrastructure.DI;
using Assets.Code.Infrastructure.EntryPoints;
using UnityEngine;
using VContainer;
using VContainer.Unity;


namespace Assets.Code.Infrastructure.Installers
{
    internal sealed class GameSceneInstaller : MonoInstaller
    {
        [SerializeField] private CameraOrbitMoveController _cameraOrbitMoveController;
        [SerializeField] private CameraTargetController _cameraTargetController;

        protected override void Install()
        {
            RegisterCommonServices();
            RegisterInputServices();
            RegisterPlayerServices();


            Builder.RegisterEntryPoint<SpaceSceneEntryPoint>();
        }

        private void RegisterCommonServices()
        {
            Builder.Register<CameraService>(Lifetime.Scoped)
                .WithParameter(_cameraOrbitMoveController)
                .WithParameter(_cameraTargetController)
                .AsImplementedInterfaces();
        }

        private void RegisterInputServices()
        {
            Builder.Register<CameraRaycaster>(Lifetime.Scoped).AsSelf();
            Builder.Register<MouseClickDetector>(Lifetime.Scoped).AsSelf().AsImplementedInterfaces();
        }

        private void RegisterPlayerServices()
        {
            Builder.Register<PlayerProvider>(Lifetime.Scoped).AsImplementedInterfaces();
        }
    }
}
