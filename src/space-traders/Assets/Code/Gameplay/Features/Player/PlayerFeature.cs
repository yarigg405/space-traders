using Assets.Code.Gameplay.Features.Player.Systems;
using Assets.Code.Infrastructure.Systems;


namespace Assets.Code.Gameplay.Features.Player
{
    public sealed class PlayerFeature : Feature
    {
        public PlayerFeature(ISystemFactory systems)
        {
            Add(systems.Create<SetupCameraOnPlayerSystem>());
            Add(systems.Create<SetPlayerDirectionByInputSystem>());
        }
    }
}
