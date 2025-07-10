using Assets.Code.Gameplay.Features.TargetCollection.Systems;
using Assets.Code.Infrastructure.Systems;


namespace Assets.Code.Gameplay.Features.TargetCollection
{
    public sealed class CollectTargetsFeature : Feature
    {
        public CollectTargetsFeature(ISystemFactory systems)
        {
            Add(systems.Create<CastForTargetsSystem>());
            Add(systems.Create<CollectTargetsIntervalSystem>());

            Add(systems.Create<CleanupTargetBuffersSystem>());
        }
    }
}
