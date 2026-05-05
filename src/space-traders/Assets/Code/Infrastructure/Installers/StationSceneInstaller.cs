using Assets.Code.Infrastructure.DI;
using Assets.Code.Infrastructure.EntryPoints;
using VContainer.Unity;


namespace Assets.Code.Infrastructure.Installers
{
    internal sealed class StationSceneInstaller : MonoInstaller
    {
        protected override void Install()
        {
            Builder.RegisterEntryPoint<SpaceStationSceneEntryPoint>();
        }
    }
}