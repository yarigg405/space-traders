using Assets.Code.Infrastructure.Systems;
using Assets.Code.ServerPart.Gameplay.Features.Synchronization.Systems;


namespace Assets.Code.ServerPart.Gameplay.Features.Synchronization
{
    public sealed class SynchronizationFeature : Feature
    {
        public SynchronizationFeature(ISystemFactory systems)
        {
            Add(systems.Create<NetworkSynchronizationSystem>());
        }
    }
}
