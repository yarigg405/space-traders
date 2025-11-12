using Assets.Code.ClientPart.Gameplay.Features.Player.Systems;
using Assets.Code.Infrastructure.Systems;


namespace Assets.Code.ClientPart.Gameplay.Features.Player
{
    public sealed class PlayerFeature : Feature
    {
        public PlayerFeature(ISystemFactory systems)
        {
            Add(systems.Create<BindPlayerServicesSystem>());


        }
    }
}
