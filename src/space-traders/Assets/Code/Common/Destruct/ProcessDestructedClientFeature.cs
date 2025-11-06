using Assets.Code.Common.Destruct.Systems;
using Assets.Code.Infrastructure.Systems;


namespace Assets.Code.Common.Destruct
{
    public sealed class ProcessDestructedClientFeature : Feature
    {
        public ProcessDestructedClientFeature(ISystemFactory systems)
        {

            Add(systems.Create<CleanupGameDestructedViewSystem>());
            Add(systems.Create<CleanupGameDestructedClientSystem>());
        }
    }
}
