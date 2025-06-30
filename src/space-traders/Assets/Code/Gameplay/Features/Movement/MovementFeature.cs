using Assets.Code.Gameplay.Features.Movement.Systems;
using Assets.Code.Infrastructure.Systems;


namespace Assets.Code.Gameplay.Features.Movement
{
    public sealed class MovementFeature : Feature
    {
        public MovementFeature(ISystemFactory systems)
        {
            Add(systems.Create<DirectionalDeltaMoveSystem>());
        }
    }
}
