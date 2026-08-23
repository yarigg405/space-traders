using Assets.Code.Common;
using Entitas;
using UnityEngine;


namespace Assets.Code.ServerPart.Gameplay.Features.Movement.Systems
{
    internal sealed class UpdateMovingSpeedSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _entities;

        public UpdateMovingSpeedSystem(GameContext game)
        {
            _entities = game.GetGroup(GameMatcher.AllOf(
                GameMatcher.MaxMoveSpeed,
                GameMatcher.CurrentSpeedModifier,
                GameMatcher.CurrentMoveSpeed
                ));
        }

        void IExecuteSystem.Execute()
        {
            foreach (var entity in _entities)
            {
                var currentSpeed = entity.CurrentMoveSpeed;
                var targetSpeed = entity.MaxMoveSpeed * entity.CurrentSpeedModifier;
                var deltaSpeed = entity.MovingAcceleration * GameConstants.FIXED_DELTA_TIME;
                var newSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, deltaSpeed);

                entity.ReplaceCurrentMoveSpeed(newSpeed);
            }
        }
    }
}
