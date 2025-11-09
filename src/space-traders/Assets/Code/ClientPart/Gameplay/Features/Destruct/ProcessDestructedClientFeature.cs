using Assets.Code.ClientPart.Gameplay.Features.Destruct.Systems;
using Assets.Code.Infrastructure.Systems;


namespace Assets.Code.ClientPart.Gameplay.Features.Destruct
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
