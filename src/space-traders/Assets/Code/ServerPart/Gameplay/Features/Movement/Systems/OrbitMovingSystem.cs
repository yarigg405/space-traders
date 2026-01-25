using Entitas;
using System.Collections.Generic;
using Unity.Mathematics;


namespace Assets.Code.ServerPart.Gameplay.Features.Movement.Systems
{
    internal sealed class OrbitMovingSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _entities;
        private readonly List<GameEntity> _buffer = new(8);
        private readonly GameContext _game;

        public OrbitMovingSystem(GameContext game)
        {
            _entities = game.GetGroup(GameMatcher.AllOf(
                GameMatcher.CurrentRotationY,
                GameMatcher.OrbitingRadius,
                GameMatcher.MovementTargetId
            ));
            _game = game;
        }

        void IExecuteSystem.Execute()
        {
            foreach (var entity in _entities)
            {
                var target = _game.GetEntityWithId(entity.MovementTargetId);
                if (target == null)
                {
                    entity.ResetMovingComponents();
                    continue;
                }

                var radius = entity.OrbitingRadius;
                var direction = entity.GlobalPosition - target.GlobalPosition;
                var distance = math.length(direction);

                if (distance <= radius) return;
                var angleOffset = math.acos(radius / distance);

                var angleToPoint = math.atan2(direction.y, direction.x);

                if (IsClockwiseMoving(entity.GlobalPosition, target.GlobalPosition, entity.CurrentRotationY))
                {
                    var tanAngle1 = angleToPoint - angleOffset;
                    var tx1 = target.GlobalPosition.x + radius * math.cos(tanAngle1);
                    var ty1 = target.GlobalPosition.y + radius * math.sin(tanAngle1);
                    var point1 = new double2(tx1, ty1);
                    var angle = MovementExtensions.GetAngleDirectionY(entity.GlobalPosition, point1);
                    entity.ReplaceTargetRotation(angle);
                }

                else
                {
                    var tanAngle2 = angleToPoint + angleOffset;
                    var tx2 = target.GlobalPosition.x + radius * math.cos(tanAngle2);
                    var ty2 = target.GlobalPosition.y + radius * math.sin(tanAngle2);
                    var point2 = new double2(tx2, ty2);
                    var angle = MovementExtensions.GetAngleDirectionY(entity.GlobalPosition, point2);
                    entity.ReplaceTargetRotation(angle);
                }
            }
        }

        private bool IsClockwiseMoving(double2 movable, double2 center, float forwardDirection)
        {
            double2 rVector = movable - center;
            var andleRadians = math.TORADIANS_DBL * forwardDirection;
            double vx = math.sin(andleRadians);
            double vy = math.cos(andleRadians);
            double cross = rVector.x * vy - rVector.y * vx;

            return cross < 0;
        }
    }
}
