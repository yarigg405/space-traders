using Assets.Code.Gameplay.Features.Movement.Systems;
using Assets.Code.Infrastructure.Systems;


namespace Assets.Code.Gameplay.Features.Movement
{
    public sealed class MovementFeature : Feature
    {
        public MovementFeature(ISystemFactory systems)
        {
            Add(systems.Create<ChaseTargetSystem>());

            Add(systems.Create<UpdateMovingSpeedSystem>());
            Add(systems.Create<RotationSystem>());
            Add(systems.Create<HandleVelocitySystem>());
            Add(systems.Create<PhysicsMovingSystem>());

            Add(systems.Create<LocalPositionRecalculateSystem>());
            Add(systems.Create<UpdateTransformRotationSystem>());
            Add(systems.Create<UpdateTransformPositionSystem>());
        }
    }
}
