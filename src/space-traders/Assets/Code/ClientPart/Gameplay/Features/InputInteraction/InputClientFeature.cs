using Assets.Code.ClientPart.Gameplay.Features.InputInteraction.Systems;
using Assets.Code.Infrastructure.Systems;


namespace Assets.Code.ClientPart.Gameplay.Features.InputInteraction
{
    public sealed class InputClientFeature : Feature
    {
        public InputClientFeature(ISystemFactory systems)
        {
            Add(systems.Create<InputCleanupSystem>());
        }
    }
}
