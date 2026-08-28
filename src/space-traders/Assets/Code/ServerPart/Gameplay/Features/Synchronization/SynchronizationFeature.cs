using Assets.Code.Infrastructure.Systems;
using Assets.Code.ServerPart.Gameplay.Features.Movement.Systems;
using Assets.Code.ServerPart.Gameplay.Features.Synchronization.Systems;


namespace Assets.Code.ServerPart.Gameplay.Features.Synchronization
{
    public sealed class SynchronizationFeature : Feature
    {
        public SynchronizationFeature(ISystemFactory systems)
        {
            Add(systems.Create<NetworkSynchronizationSystem>());
            Add(systems.Create<DockingProcessSynchronizationSystem>());
            Add(systems.Create<SendPlayerSnapshotSystem>());

            Add(systems.Create<SynchronizationCleanupSystem>());
        }
    }
}
