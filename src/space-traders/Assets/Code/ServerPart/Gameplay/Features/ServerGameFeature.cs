using Assets.Code.Infrastructure.Systems;
using Assets.Code.ServerPart.Gameplay.Features.Destruct;
using Assets.Code.ServerPart.Gameplay.Features.Movement;


namespace Assets.Code.ServerPart.Gameplay.Features
{
    public sealed class ServerGameFeature : Feature
    {
        public ServerGameFeature(ISystemFactory systems)
        {
            Add(systems.Create<MovementServerFeature>());

            Add(systems.Create<ProcessDestructedServerFeature>());
        }
    }
}
