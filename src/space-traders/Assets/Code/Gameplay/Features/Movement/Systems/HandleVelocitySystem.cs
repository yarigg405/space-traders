using Assets.Code.Gameplay.Common.Time;
using Entitas;
using UnityEngine;


namespace Assets.Code.Gameplay.Features.Movement.Systems
{
    internal sealed class HandleVelocitySystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _entities;
        private readonly ITimeService _time;

        internal HandleVelocitySystem(GameContext game, ITimeService time)
        {
            _entities = game.GetGroup(GameMatcher.AllOf(
                GameMatcher.CurrentRotationY,
                GameMatcher.CurrentMoveSpeed,
                GameMatcher.Velocity,
                GameMatcher.VelocityAgility
            ));
            _time = time;
        }

        void IExecuteSystem.Execute()
        {
            foreach (var entity in _entities)
            {
                var currentRadAngle = entity.CurrentRotationY * Mathf.Deg2Rad;

                var targetVelocity = new Vector2(
                    Mathf.Sin(currentRadAngle),
                    Mathf.Cos(currentRadAngle)) * entity.CurrentMoveSpeed;

                var newVelocity = Vector3.MoveTowards(entity.Velocity, targetVelocity, entity.VelocityAgility * _time.DeltaTime);
                entity.ReplaceVelocity(newVelocity);
            }
        }
    }
}