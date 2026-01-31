using Assets.Code.Common;
using Assets.Code.Common.DataContainers;
using Entitas;
using System.Collections.Generic;
using Yrr.Utils;


namespace Assets.Code.ServerPart.Gameplay.Features.Movement.Systems
{
    internal sealed class WarpPreparationSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _entities;
        private readonly List<GameEntity> _buffer = new(32);

        internal WarpPreparationSystem(GameContext game)
        {
            _entities = game.GetGroup(GameMatcher.AllOf(
                GameMatcher.WarpPreparation,
                GameMatcher.WarpFinishCoordinates
            ));
        }

        void IExecuteSystem.Execute()
        {
            foreach (var entity in _entities.GetEntities(_buffer))
            {
                var direction = CommonExtensions.GetDirectionAngleFromTo(entity.GlobalPosition, entity.WarpFinishCoordinates);
                entity.ReplaceTargetRotation(direction);
                entity.ReplaceCurrentSpeedModifier(1f);


                if (entity.CurrentSpeedModifier > 0.3)
                {
                    var deltaAngle = AnglesUtil.GetMinAngledDelta(entity.TargetRotation, entity.CurrentRotationY);
                    if (deltaAngle < 5f)
                    {
                        var container = new WarpDataContainer(entity.GlobalPosition, entity.WarpFinishCoordinates);                        

                        entity.ReplaceWarpDataContainer(container);

                        entity.RemoveWarpFinishCoordinates();
                        entity.isWarpPreparation = false;
                        entity.isMoving = false;
                    }
                }
            }
        }
    }
}
