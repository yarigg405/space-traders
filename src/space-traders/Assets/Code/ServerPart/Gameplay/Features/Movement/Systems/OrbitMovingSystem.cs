using Entitas;
using Unity.Mathematics;
using UnityEngine;
using Yrr.Utils;


namespace Assets.Code.ServerPart.Gameplay.Features.Movement.Systems
{
    internal sealed class OrbitMovingSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _entities;

        public OrbitMovingSystem(GameContext game)
        {
            _entities = game.GetGroup(GameMatcher.AllOf(
                GameMatcher.CurrentRotationY,
                GameMatcher.OrbitingRadius,
                GameMatcher.MovementTarget
            ));
        }

        void IExecuteSystem.Execute()
        {
            foreach (var entity in _entities)
            {
                var radius = entity.OrbitingRadius;
                var direction = entity.Transform.position - entity.MovementTarget.Transform.position;
                var distance = direction.magnitude;

                if (distance <= radius) return;
                var angleOffset = Mathf.Acos(radius / distance);

                var angleToPoint = Mathf.Atan2(direction.z, direction.x);

                if (IsClockwiseMoving(entity.GlobalPosition, entity.GlobalPosition, entity.CurrentRotationY))
                {
                    var tanAngle1 = angleToPoint - angleOffset;
                    var tx1 = entity.MovementTarget.Transform.position.x + radius * Mathf.Cos(tanAngle1);
                    var ty1 = entity.MovementTarget.Transform.position.z + radius * Mathf.Sin(tanAngle1);
                    var point1 = new Vector3(tx1, 0, ty1);
                    var angle = AnglesUtil.GetAngleDirectionY(entity.Transform.position, point1);
                    entity.ReplaceTargetRotation(angle);
                }

                else
                {
                    var tanAngle2 = angleToPoint + angleOffset;
                    var tx2 = entity.MovementTarget.Transform.position.x + radius * Mathf.Cos(tanAngle2);
                    var ty2 = entity.MovementTarget.Transform.position.z + radius * Mathf.Sin(tanAngle2);
                    var point2 = new Vector3(tx2, 0, ty2);
                    var angle = AnglesUtil.GetAngleDirectionY(entity.Transform.position, point2);
                    entity.ReplaceTargetRotation(angle);
                }
            }
        }

        private bool IsClockwiseMoving(double2 from, double2 to, float forwardDirection)
        {
            var directionVector = from - to;
            var rad = forwardDirection * math.TORADIANS_DBL;
            var movingDirection = new double2(math.sin(rad), math.cos(rad));
            var cross = movingDirection.x * movingDirection.y - directionVector.y * directionVector.x;

            return cross < 0f;
        }
    }
}
