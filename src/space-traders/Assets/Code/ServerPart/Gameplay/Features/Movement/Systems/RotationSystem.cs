using Assets.Code.Common.Time;
using Entitas;
using UnityEngine;
using Yrr.Utils;


namespace Assets.Code.ServerPart.Gameplay.Features.Movement.Systems
{
    internal sealed class RotationSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _entities;
        private readonly ITimeService _time;

        public RotationSystem(GameContext game, ITimeService time)
        {
            _entities = game.GetGroup(GameMatcher.AllOf(
                GameMatcher.CurrentRotationY,
                GameMatcher.TargetRotation,
                GameMatcher.RotationSpeed
                ));
            _time = time;
        }

        void IExecuteSystem.Execute()
        {
            foreach (var entity in _entities)
            {
                var current = entity.CurrentRotationY;
                var target = entity.TargetRotation;
                var delta = Mathf.DeltaAngle(current, target);

                var speedFactor = Mathf.Abs(delta) * .01f;
                var maxRotationSpeed = entity.RotationSpeed * _time.DeltaTime;
                var effectiveRotationSpeed = maxRotationSpeed * speedFactor;

                var newAngle = AnglesUtil.MoveTowardsAngle(current, target, effectiveRotationSpeed);

                entity.ReplaceCurrentRotationY(newAngle);
                entity.isNeedSynchronize = true;
            }
        }
    }
}
