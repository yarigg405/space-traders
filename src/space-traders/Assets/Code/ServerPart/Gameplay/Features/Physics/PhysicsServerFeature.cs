using Assets.Code.Infrastructure.Systems;
using Assets.Code.ServerPart.Gameplay.Features.Physics.Systems;


namespace Assets.Code.ServerPart.Gameplay.Features.Physics
{
    public sealed class PhysicsServerFeature : Feature
    {
        public PhysicsServerFeature(ISystemFactory systems)
        {
            Add(systems.Create<PhysicPushingSystem>());
            Add(systems.Create<CollectCollisionsIntervalSystem>());
            Add(systems.Create<CastForTriggersInteractionsSystem>());
            Add(systems.Create<TriggerEventHandlersSystem>());
        }
    }
}
