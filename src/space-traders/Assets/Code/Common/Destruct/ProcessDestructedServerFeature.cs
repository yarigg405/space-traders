using Assets.Code.Common.Destruct.Systems;
using Assets.Code.Infrastructure.Systems;


namespace Assets.Code.Common.Destruct
{
    public sealed class ProcessDestructedServerFeature : Feature
    {
        public ProcessDestructedServerFeature(ISystemFactory systems)
        {
            Add(systems.Create<SelfDestructTimerSystem>());
            Add(systems.Create<CleanupGameDestructedServerSystem>());
        }
    }
}
