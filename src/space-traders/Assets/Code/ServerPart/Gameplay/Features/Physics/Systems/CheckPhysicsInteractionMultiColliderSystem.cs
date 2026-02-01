using Entitas;
using System;


namespace Assets.Code.ServerPart.Gameplay.Features.Physics.Systems
{
    internal sealed class CheckPhysicsInteractionMultiColliderSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _entities;

        public CheckPhysicsInteractionMultiColliderSystem(GameContext game)
        {
            _entities = game.GetGroup(GameMatcher.AllOf(
                GameMatcher.CollidersBuffer,
                GameMatcher.PhysicRadius
                ));
        }

        void IExecuteSystem.Execute()
        {
         here  
        }
    }
}
