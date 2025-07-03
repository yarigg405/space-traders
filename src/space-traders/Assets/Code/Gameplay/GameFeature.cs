using Assets.Code.Gameplay.Features.Movement;
using Assets.Code.Gameplay.Features.Player;
using Assets.Code.Gameplay.InputInteraction;
using Assets.Code.Infrastructure.Systems;


namespace Assets.Code.Gameplay
{
    internal sealed class GameFeature : Feature
    {
        public GameFeature(ISystemFactory systems)
        {
            Add(systems.Create<InputFeature>());
            Add(systems.Create<MovementFeature>());
            Add(systems.Create<PlayerFeature>());
        }
    }
}
