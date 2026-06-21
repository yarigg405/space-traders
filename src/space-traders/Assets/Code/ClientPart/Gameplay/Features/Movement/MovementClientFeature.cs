using Assets.Code.ClientPart.Gameplay.Features.Movement.Systems;
using Assets.Code.ClientPart.Gameplay.Features.Player.Systems;
using Assets.Code.Infrastructure.Systems;
using Assets.Code.ServerPart.Gameplay.Features.Movement.Systems;
using Assets.Code.ServerPart.Gameplay.Features.SkyboxObjects.Systems;


namespace Assets.Code.ClientPart.Gameplay.Features.Movement
{
    public sealed class MovementClientFeature : Feature
    {
        public MovementClientFeature(ISystemFactory systems)
        {
            Add(systems.Create<PlayerPreviousFramePositionSystem>());
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
            Add(systems.Create<UpdateLocalPositionSystem>());
            Add(systems.Create<UpdateViewModelSystem>());

            Add(systems.Create<UpdateSkyboxSpaceStateSystem>());
            Add(systems.Create<UpdateSkyboxLocalPositionSystem>());
            Add(systems.Create<UpdateTransformRotationSystem>());
            Add(systems.Create<UpdateTransformPositionSystem>());
        }
    }
}
