using Assets.Code.ClientPart.Gameplay.Features.InputInteraction.Systems;
using Assets.Code.Common.Time.Systems;
using Assets.Code.Infrastructure.Systems;
using Assets.Code.ServerPart.Gameplay.Features.Movement.Systems;


namespace Assets.Code.ClientPart.Gameplay.Features.Movement
{
    public sealed class SimulationMovementFeature : Feature
    {
        public SimulationMovementFeature(ISystemFactory systems)
        {
            Add(systems.Create<TickIncrementSystem>());
            Add(systems.Create<CapturePreviousTickStateSystem>());

            Add(systems.Create<PlayerCommandSystem>());
            Add(systems.Create<PredictPlayerInputSystem>());

            Add(systems.Create<OrbitMovingSystem>());
            Add(systems.Create<KeepDistanceSystem>());

            Add(systems.Create<UpdateMovingSpeedSystem>());
            Add(systems.Create<RotationSystem>());
            Add(systems.Create<HandleVelocitySystem>());
            Add(systems.Create<PhysicsMovingSystem>());

            Add(systems.Create<WarpPreparationSystem>());
            Add(systems.Create<WarpMovingSystem>());
            Add(systems.Create<ShipStartDockingSystem>());
            Add(systems.Create<ShipStartUndockingSystem>());
            Add(systems.Create<ShipFinishUndockSystem>());

            Add(systems.Create<UpdateQuadrantIndexSystem>());
        }
    }
}
