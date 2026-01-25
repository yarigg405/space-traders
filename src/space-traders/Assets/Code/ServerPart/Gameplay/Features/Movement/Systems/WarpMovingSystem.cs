using Assets.Code.Common;
using Assets.Code.Common.Time;
using Entitas;
using System.Collections.Generic;
using Unity.Mathematics;


namespace Assets.Code.ServerPart.Gameplay.Features.Movement.Systems
{
    internal sealed class WarpMovingSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _entities;
        private readonly List<GameEntity> _buffer = new(64);
        private readonly ITimeService _time;

        public WarpMovingSystem(GameContext game, ITimeService time)
        {
            _time = time;

            _entities = game.GetGroup(GameMatcher.AllOf(
                GameMatcher.WarpDataContainer,
                GameMatcher.GlobalPosition
            ));
        }

        void IExecuteSystem.Execute()
        {
            foreach (var entity in _entities.GetEntities(_buffer))
            {
                var container = entity.WarpDataContainer;
                var currentDistance = (container.WarpFinishPosition - entity.GlobalPosition).Magnitude();
                var distanceModifier = currentDistance / container.WarpTotalDistance;


                if (distanceModifier > 0.3)
                {
                    container.Acceleration += _time.DeltaTime * 0.65;
                    container.WarpSpeed += math.pow(container.Acceleration, 4);
                    container.TopSpeed = container.WarpSpeed;
                    container.TopDistance = currentDistance;

                    var deltaMove = container.WarpSpeed * _time.DeltaTime;
                    var newPos = CommonExtensions
                        .MoveTowards(entity.GlobalPosition, container.WarpFinishPosition, deltaMove);
                    entity.ReplaceGlobalPosition(newPos);
                }

                else
                {
                    var t = currentDistance / container.TopDistance;
                    container.WarpSpeed =
                        CommonExtensions.DoubleLerp(entity.MaxMoveSpeed * entity.CurrentSpeedModifier * 0.5f,
                        container.TopSpeed, t);

                    var deltaMove = container.WarpSpeed * _time.DeltaTime;
                    var newPos = CommonExtensions
                         .MoveTowards(entity.GlobalPosition, container.WarpFinishPosition, deltaMove);
                    entity.ReplaceGlobalPosition(newPos);
                }

                if (currentDistance < 100)
                {
                    entity.SetWarpFinished();
                }
            }
        }
    }
}
