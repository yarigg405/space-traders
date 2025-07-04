using Entitas;
using UnityEngine;


namespace Assets.Code.Gameplay.Features.Movement.Systems
{
    internal sealed class UpdateTransformRotationSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _entities;

        internal UpdateTransformRotationSystem(GameContext game)
        {
            _entities = game.GetGroup(GameMatcher.AllOf(
                GameMatcher.Transform,
                GameMatcher.CurrentRotationY
            ));
        }

        void IExecuteSystem.Execute()
        {
            foreach (var entity in _entities)
            {
                entity.Transform.rotation = Quaternion.Euler(0, entity.CurrentRotationY, 0);
            }
        }
    }
}