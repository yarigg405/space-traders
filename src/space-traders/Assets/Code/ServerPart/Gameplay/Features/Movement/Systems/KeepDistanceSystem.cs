using Entitas;
using System.Collections.Generic;
using Unity.Mathematics;


namespace Assets.Code.ServerPart.Gameplay.Features.Movement.Systems
{
    internal sealed class KeepDistanceSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _entities;
        private readonly List<GameEntity> _buffer = new(8);
        private readonly GameContext _game;

        public KeepDistanceSystem(GameContext game)
        {
            _entities = game.GetGroup(GameMatcher.AllOf(
                GameMatcher.MovementTargetId,
                GameMatcher.KeepDistanceMinMax
                ));
            _game = game;
        }

        void IExecuteSystem.Execute()
        {
            foreach (var entity in _entities.GetEntities(_buffer))
            {
                var target = _game.GetEntityWithId(entity.MovementTargetId);
                if (target == null)
                {
                    entity.ResetMovingComponents();
                    continue;
                }

                var minMaxDistance = entity.KeepDistanceMinMax;

                var distance = math.length(entity.GlobalPosition - target.GlobalPosition);

                if (distance < minMaxDistance.x)
                {
                    var angle = MovementExtensions.GetAngleDirectionY(target.GlobalPosition, entity.GlobalPosition);
                    entity.ReplaceTargetRotation(angle);

                    if (entity.CurrentSpeedModifier == 0)
                        entity.ReplaceCurrentSpeedModifier(1f);
                }

                else if (distance > minMaxDistance.y)
                {
                    var angle = MovementExtensions.GetAngleDirectionY(entity.GlobalPosition, target.GlobalPosition);
                    entity.ReplaceTargetRotation(angle);

                    if (entity.CurrentSpeedModifier == 0)
                        entity.ReplaceCurrentSpeedModifier(1f);
                }

                else
                {
                    entity.ReplaceCurrentSpeedModifier(0);
                }
            }
        }
    }
}
