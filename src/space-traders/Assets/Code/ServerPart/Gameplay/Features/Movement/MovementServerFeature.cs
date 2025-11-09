using Assets.Code.Infrastructure.Systems;
using Assets.Code.ServerPart.Gameplay.Features.Movement.Systems;


namespace Assets.Code.ServerPart.Gameplay.Features.Movement
{
    public sealed class MovementServerFeature : Feature
    {
        public MovementServerFeature(ISystemFactory systems)
        {
            Add(systems.Create<GlobalPositionSynchronizeSystem>());
        }
    }
}
