using Assets.Code.ClientPart.Gameplay.Features.InputInteraction.Systems;
using Assets.Code.Infrastructure.Systems;


namespace Assets.Code.ClientPart.Gameplay.Features.InputInteraction
{
    public sealed class InputFeature : Feature
    {
        public InputFeature(ISystemFactory systems)
        {
            Add(systems.Create<InputListenClientSystem>());

            Add(systems.Create<InputCleanupSystem>());
        }
    }
}
