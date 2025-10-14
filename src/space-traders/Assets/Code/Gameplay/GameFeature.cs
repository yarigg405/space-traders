using Assets.Code.Gameplay.Features.Player;
using Assets.Code.Infrastructure.Systems;
using Assets.Code.View;


namespace Assets.Code.Gameplay
{
    internal sealed class GameFeature : Feature
    {
        public GameFeature(ISystemFactory systems)
        {
            Add(systems.Create<BindViewFeature>());
            Add(systems.Create<PlayerFeature>());
        }
    }
}
