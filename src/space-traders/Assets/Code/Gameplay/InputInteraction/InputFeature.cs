using Assets.Code.Gameplay.InputInteraction.Systems;
using Assets.Code.Infrastructure.Systems;


namespace Assets.Code.Gameplay.InputInteraction
{
    public sealed class InputFeature : Feature
    {
        public InputFeature(ISystemFactory systems)
        {
            Add(systems.Create<InitializeInputSystem>());

            Add(systems.Create<EmitInputSystem>());
        }
    }
}
