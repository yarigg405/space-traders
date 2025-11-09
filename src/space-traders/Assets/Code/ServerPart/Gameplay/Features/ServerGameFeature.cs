using Assets.Code.Infrastructure.Systems;
using Assets.Code.ServerPart.Gameplay.Features.Destruct;


namespace Assets.Code.ServerPart.Gameplay.Features
{
    public sealed class ServerGameFeature : Feature
    {
        public ServerGameFeature(ISystemFactory systems)
        {


            Add(systems.Create<ProcessDestructedServerFeature>());
        }
    }
}
