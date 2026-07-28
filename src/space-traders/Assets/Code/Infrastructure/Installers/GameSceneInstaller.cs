using Assets.Code.ClientPart.CameraSystem;
using Assets.Code.ClientPart.Gameplay.Features.InputInteraction;
using Assets.Code.ClientPart.Gameplay.Features.Navigation;
using Assets.Code.ClientPart.Gameplay.Features.Player.Infrastructure;
using Assets.Code.ClientPart.View;
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
        [SerializeField] private PlayerWarpEffectView _playerWarpEffectView;

        protected override void Install()
        {
            RegisterCommonServices();
            RegisterInputServices();
            RegisterNavigationServices();
            RegisterPlayerServices();

            Builder.RegisterEntryPoint<SpaceSceneConfigApplier>();
            Builder.RegisterEntryPoint<SpaceSceneEntryPoint>();
        }

        private void RegisterCommonServices()
        {
            Builder.RegisterInstance(_cameraOrbitMoveController);
            Builder.RegisterInstance(_cameraTargetController);
            Builder.RegisterInstance(_skyboxCamera);
            Builder.RegisterInstance(_playerWarpEffectView);

            Builder.Register<CameraService>(Lifetime.Scoped).AsImplementedInterfaces();
            Builder.Register<PlayerWarpEffectController>(Lifetime.Scoped).AsImplementedInterfaces();
            Builder.Register<EntityViewFactory>(Lifetime.Singleton).AsImplementedInterfaces();
            Builder.Register<SkyboxSpaceState>(Lifetime.Scoped);
            Builder.Register<ParticlesHandler>(Lifetime.Scoped).AsSelf().AsImplementedInterfaces();
        }

        private void RegisterInputServices()
        {
            Builder.Register<CameraRaycaster>(Lifetime.Scoped).AsSelf();
            Builder.Register<MouseClickDetector>(Lifetime.Scoped).AsSelf().AsImplementedInterfaces();
        }

        private void RegisterNavigationServices()
        {
            Builder.Register<SelectionService>(Lifetime.Scoped).AsSelf();
            Builder.Register<NavigationRegistry>(Lifetime.Scoped).AsSelf().AsImplementedInterfaces();
            Builder.Register<NavigationSelectionRouter>(Lifetime.Scoped).AsImplementedInterfaces();
        }

        private void RegisterPlayerServices()
        {
            Builder.Register<PlayerProvider>(Lifetime.Scoped).AsImplementedInterfaces();
            Builder.Register<PlayerShipController>(Lifetime.Scoped).AsSelf();
        }
    }
}
