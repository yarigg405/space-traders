using Assets.Code.ClientPart.CameraSystem;
using Assets.Code.ClientPart.Gameplay.Features.InputInteraction;
using Assets.Code.ClientPart.Gameplay.Features.Player.Infrastructure;
using Assets.Code.ClientPart.View.Factory;
using Assets.Code.ClientPart.Visual;
using Assets.Code.ClientPart.Visual.Player;
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
        [SerializeField] private SkyboxCamera _skyboxCamera;

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
                .WithParameter(_skyboxCamera)
                .AsImplementedInterfaces();


            Builder.Register<EntityViewFactory>(Lifetime.Singleton).AsImplementedInterfaces();
            Builder.Register<SkyboxSpaceState>(Lifetime.Scoped);
            Builder.Register<ParticlesHandler>(Lifetime.Scoped).AsSelf().AsImplementedInterfaces();
        }

        private void RegisterInputServices()
        {
            Builder.Register<CameraRaycaster>(Lifetime.Scoped).AsSelf();
            Builder.Register<MouseClickDetector>(Lifetime.Scoped).AsSelf().AsImplementedInterfaces();
        }

        private void RegisterPlayerServices()
        {
            Builder.Register<PlayerProvider>(Lifetime.Scoped).AsImplementedInterfaces();
            Builder.Register<PlayerShipController>(Lifetime.Scoped).AsSelf();
            Builder.Register<PlayerQuadrantChangeObserver>(Lifetime.Scoped).AsSelf();
        }
    }
}
