using Assets.Code.Infrastructure.Systems;
using Assets.Code.ServerPart.Gameplay.Features.InputInteraction.Systems;


namespace Assets.Code.ServerPart.Gameplay.Features.InputInteraction
{
    internal sealed class InputServerFeature : Feature
    {
        public InputServerFeature(ISystemFactory systems)
        {
            Add(systems.Create<SetPlayerDirectionByInputSystem>());
            Add(systems.Create<SetPlayerSpeedByInputSystem>());
        }
    }
}
