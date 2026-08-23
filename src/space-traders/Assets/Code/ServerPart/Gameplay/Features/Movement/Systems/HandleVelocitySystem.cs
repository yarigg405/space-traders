using Assets.Code.Common;
using Entitas;
using UnityEngine;


namespace Assets.Code.ServerPart.Gameplay.Features.Movement.Systems
{
    internal sealed class HandleVelocitySystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _entities;

        public HandleVelocitySystem(GameContext game)
        {
            _entities = game.GetGroup(GameMatcher.AllOf(
                GameMatcher.CurrentRotationY,
                GameMatcher.CurrentMoveSpeed,
                GameMatcher.VelocityAgility
            ));
        }

        void IExecuteSystem.Execute()
        {
            foreach (var entity in _entities)
            {
                var currentRadAngle = entity.CurrentRotationY * Mathf.Deg2Rad;

                var targetVelocity = new Vector2(
                    Mathf.Sin(currentRadAngle),
                    Mathf.Cos(currentRadAngle)) * entity.CurrentMoveSpeed;

                var newVelocity = Vector3.MoveTowards(entity.Velocity, targetVelocity, entity.VelocityAgility * GameConstants.FIXED_DELTA_TIME);
                entity.ReplaceVelocity(newVelocity);
            }
        }
    }
}
