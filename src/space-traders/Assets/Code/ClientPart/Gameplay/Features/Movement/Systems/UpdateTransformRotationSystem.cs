using Assets.Code.Common.Time;
using Entitas;
using UnityEngine;


namespace Assets.Code.ClientPart.Gameplay.Features.Movement.Systems
{
    internal sealed class UpdateTransformRotationSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _entities;
        private readonly InterpolationClock _interpolation;

        public UpdateTransformRotationSystem(GameContext game, InterpolationClock interpolation)
        {
            _entities = game.GetGroup(GameMatcher.AllOf(
                GameMatcher.Transform,
                GameMatcher.CurrentRotationY
            ));
            _interpolation = interpolation;
        }

        void IExecuteSystem.Execute()
        {
            foreach (var entity in _entities)
            {
                var a = _interpolation.Alpha;
                var rot = entity.hasPreviousTickRotationY
                    ? Mathf.LerpAngle(entity.PreviousTickRotationY, entity.CurrentRotationY, a)
                    : entity.CurrentRotationY;
                entity.Transform.rotation = Quaternion.Euler(0, rot, 0);
            }
        }
    }
}
