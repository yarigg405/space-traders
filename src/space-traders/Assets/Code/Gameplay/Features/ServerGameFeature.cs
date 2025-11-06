using Assets.Code.Common.Destruct;
using Assets.Code.Infrastructure.Systems;


namespace Assets.Code.Gameplay.Features
{
    public sealed class ServerGameFeature : Feature
    {
        public ServerGameFeature(ISystemFactory systems)
        {


            Add(systems.Create<ProcessDestructedServerFeature>());
        }
    }
}
