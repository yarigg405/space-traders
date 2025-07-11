using Assets.Code.Gameplay.Common.Time;
using Entitas;
using UnityEngine;


namespace Assets.Code.Gameplay.Features.Movement.Systems
{
    internal sealed class UpdateMovingSpeedSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _entities;
        private readonly ITimeService _time;

        internal UpdateMovingSpeedSystem(GameContext game, ITimeService time)
        {
            _entities = game.GetGroup(GameMatcher.AllOf(
                GameMatcher.MaxMoveSpeed,
                GameMatcher.CurrentSpeedModifier,
                GameMatcher.CurrentMoveSpeed
            ));
            _time = time;
        }

        void IExecuteSystem.Execute()
        {
            foreach (var entity in _entities)
            {
                var currentSpeed = entity.CurrentMoveSpeed;
                var targetSpeed = entity.MaxMoveSpeed * entity.CurrentSpeedModifier;
                var deltaSpeed = entity.MovingAcceleration * _time.DeltaTime;
                var newSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, deltaSpeed);

                entity.ReplaceCurrentMoveSpeed(newSpeed);
            }
        }
    }
}