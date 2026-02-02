using Entitas;


namespace Assets.Code.ServerPart.Gameplay.Features.Physics.Systems
{
    internal sealed class CheckPhysicsInteractionSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _entities;

        public CheckPhysicsInteractionSystem(GameContext game)
        {
            _entities = game.GetGroup(GameMatcher.AllOf(
                GameMatcher.CollidersBuffer,
                GameMatcher.PhysicRadius
                ));
        }


        void IExecuteSystem.Execute()
        {
            
        }
    }
}
