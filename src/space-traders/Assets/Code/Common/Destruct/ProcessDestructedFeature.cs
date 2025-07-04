using Assets.Code.Common.Destruct.Systems;
using Assets.Code.Infrastructure.Systems;


namespace Assets.Code.Common.Destruct
{
    public sealed class ProcessDestructedFeature : Feature
    {
        public ProcessDestructedFeature(ISystemFactory systems)
        {
            Add(systems.Create<SelfDestructTimerSystem>());
            Add(systems.Create<CleanupGameDestructedViewSystem>());
            Add(systems.Create<CleanupGameDestructedSystem>());
        }
    }
}
