using Assets.Code.Infrastructure.Systems;
using Assets.Code.ServerPart.Gameplay.Features.Physics.Systems;


namespace Assets.Code.ServerPart.Gameplay.Features.Physics
{
    public sealed class PhysicsServerFeature : Feature
    {
        public PhysicsServerFeature(ISystemFactory systems)
        {
            Add(systems.Create<UpdateQuadrantIndexSystem>());
            Add(systems.Create<UpdatePhysicsQuadrantsSystem>());

            Add(systems.Create<InitializeStationsSystem>());

            Add(systems.Create<CheckPhysicsInteractionMultiColliderSystem>());
            Add(systems.Create<CheckPhysicsInteractionSystem>());
            Add(systems.Create<PhysicsInteractionCleanupSystem>());
        }
    }
}
