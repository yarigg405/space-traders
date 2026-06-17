using Assets.Code.ClientPart.Gameplay.Features.InputInteraction.Systems;
using Assets.Code.Infrastructure.Systems;
using Assets.Code.ServerPart.Gameplay.Features.Destruct.Systems;


namespace Assets.Code.ServerPart.Gameplay.Features.Destruct
{
    public sealed class ProcessDestructedServerFeature : Feature
    {
        public ProcessDestructedServerFeature(ISystemFactory systems)
        {
            Add(systems.Create<SelfDestructTimerSystem>());
            Add(systems.Create<CleanupGameDestructedServerSystem>());
            Add(systems.Create<PreDestructionEntityHandleSystem>());
            Add(systems.Create<InputCleanupSystem>());
        }
    }
}
