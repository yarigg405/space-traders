using Entitas;
using UnityEngine;


namespace Assets.Code.Gameplay.Features.Movement.Systems
{
    internal sealed class UpdateTransformSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _entities;

        internal UpdateTransformSystem(GameContext game)
        {
            _entities = game.GetGroup(GameMatcher.AllOf(
                GameMatcher.Transform,
                GameMatcher.LocalPosition,
                GameMatcher.CurrentRotationY
            ));
        }

        void IExecuteSystem.Execute()
        {
            foreach (var entity in _entities)
            {
                entity.Transform.position = entity.LocalPosition;
                entity.Transform.rotation = Quaternion.Euler(0, entity.CurrentRotationY, 0);
            }
        }
    }
}