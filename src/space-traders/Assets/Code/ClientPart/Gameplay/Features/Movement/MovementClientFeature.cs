using Assets.Code.ClientPart.Gameplay.Features.Movement.Systems;
using Assets.Code.Infrastructure.Systems;
using Assets.Code.ServerPart.Gameplay.Features.Movement.Systems;


namespace Assets.Code.ClientPart.Gameplay.Features.Movement
{
    public sealed class MovementClientFeature : Feature
    {
        public MovementClientFeature(ISystemFactory systems)
        {
            Add(systems.Create<UpdateQuadrantIndexSystem>());
            Add(systems.Create<UpdateLocalPositionSystem>());
            Add(systems.Create<UpdateTransformRotationSystem>());
            Add(systems.Create<UpdateTransformPositionSystem>());
        }
    }
}
