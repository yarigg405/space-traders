using Assets.Code.Infrastructure.Systems;
using Assets.Code.ServerPart.Gameplay.Features.Destruct;
using Assets.Code.ServerPart.Gameplay.Features.InputInteraction;
using Assets.Code.ServerPart.Gameplay.Features.Movement;
using Assets.Code.ServerPart.Gameplay.Features.Physics;
using Assets.Code.ServerPart.Gameplay.Features.Synchronization;


namespace Assets.Code.ServerPart.Gameplay.Features
{
    public sealed class ServerGameFeature : Feature
    {
        public ServerGameFeature(ISystemFactory systems)
        {
            Add(systems.Create<InputServerFeature>());
            Add(systems.Create<MovementServerFeature>());
            Add(systems.Create<PhysicsServerFeature>());

            Add(systems.Create<SynchronizationFeature>());

            Add(systems.Create<ProcessDestructedServerFeature>());
        }
    }
}
