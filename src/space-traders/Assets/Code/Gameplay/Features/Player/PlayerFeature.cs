using Assets.Code.Gameplay.Features.Player.Systems;
using Assets.Code.Infrastructure.Systems;


namespace Assets.Code.Gameplay.Features.Player
{
    public sealed class PlayerFeature : Feature
    {
        public PlayerFeature(ISystemFactory systems)
        {
            Add(systems.Create<InitializeHeroSystem>());
            Add(systems.Create<SetPlayerDirectionByInputSystem>());
        }
    }
}
