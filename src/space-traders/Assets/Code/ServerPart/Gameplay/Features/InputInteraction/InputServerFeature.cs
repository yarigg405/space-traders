using Assets.Code.Infrastructure.Systems;
using Assets.Code.ServerPart.Gameplay.Features.InputInteraction.Systems;
using Assets.Code.ServerPart.Gameplay.Features.Physics.Systems;


namespace Assets.Code.ServerPart.Gameplay.Features.InputInteraction
{
    internal sealed class InputServerFeature : Feature
    {
        public InputServerFeature(ISystemFactory systems)
        {
            Add(systems.Create<SetPlayerKeepDistanceSystem>());
            Add(systems.Create<SetPlayerOrbitMoveSystem>());
            Add(systems.Create<SetWarpMovingByInputSystem>());
        }
    }
}
