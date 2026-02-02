using Entitas;
using System;


namespace Assets.Code.ServerPart.Gameplay.Features.Physics.Systems
{
    internal sealed class CheckPhysicsInteractionMultiColliderSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _entities;
        private readonly IGroup<GameEntity> _multicollidersEntities;

        public CheckPhysicsInteractionMultiColliderSystem(GameContext game)
        {
            _entities = game.GetGroup(GameMatcher.AllOf(
                GameMatcher.CollidersBuffer,
                GameMatcher.PhysicRadius
                ));

            _multicollidersEntities = game.GetGroup(GameMatcher.ChildrenColliders);
        }

        void IExecuteSystem.Execute()
        {
          
        }
    }
}
