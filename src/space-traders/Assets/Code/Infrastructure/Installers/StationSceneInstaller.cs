using Assets.Code.ClientPart.CameraSystem;
using Assets.Code.ClientPart.Gameplay.Features.Player.Infrastructure;
using Assets.Code.ClientPart.View;
using Assets.Code.ClientPart.View.Factory;
using Assets.Code.ClientPart.Visual;
using Assets.Code.Infrastructure.DI;
using Assets.Code.Infrastructure.EntryPoints;
using UnityEngine;
using VContainer;
using VContainer.Unity;


namespace Assets.Code.Infrastructure.Installers
{
    internal sealed class StationSceneInstaller : MonoInstaller
    {
        [SerializeField] private CameraTargetController _cameraTargetController;

        protected override void Install()
        {
            Builder.RegisterInstance(_cameraTargetController);
            Builder.Register<StationCameraService>(Lifetime.Scoped).AsImplementedInterfaces();

            Builder.Register<ShipViewFactory>(Lifetime.Singleton).AsImplementedInterfaces();
            Builder.Register<StationVisualApplier>(Lifetime.Singleton).AsSelf();

            Builder.Register<PlayerProvider>(Lifetime.Scoped).AsImplementedInterfaces();
            Builder.Register<ParticlesHandler>(Lifetime.Scoped).AsSelf();

            Builder.RegisterEntryPoint<SpaceStationSceneEntryPoint>();
        }
    }
}