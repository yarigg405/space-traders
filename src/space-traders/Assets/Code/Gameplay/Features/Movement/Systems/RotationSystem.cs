using Assets.Code.Gameplay.Common.Time;
using Entitas;
using Yrr.Utils;


namespace Assets.Code.Gameplay.Features.Movement.Systems
{
    internal sealed class RotationSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _entities;
        private readonly ITimeService _time;

        internal RotationSystem(GameContext game, ITimeService time)
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
                var newAngle = AnglesUtil.MoveTowardsAngle(
                    entity.CurrentRotationY,
                    entity.TargetRotation,
                    entity.RotationSpeed * _time.DeltaTime);
                entity.ReplaceCurrentRotationY(newAngle);
            }
        }
    }
}