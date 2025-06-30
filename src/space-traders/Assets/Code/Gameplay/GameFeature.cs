using Assets.Code.Gameplay.Features.Movement;
using Assets.Code.Infrastructure.Systems;


namespace Assets.Code.Gameplay
{
    internal sealed class GameFeature : Feature
    {
        public GameFeature(ISystemFactory systems)
        {
            Add(systems.Create<MovementFeature>());
        }
    }
}
