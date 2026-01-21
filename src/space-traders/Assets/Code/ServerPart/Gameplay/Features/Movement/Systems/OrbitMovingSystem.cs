using Entitas;
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

                if (IsClockwiseMoving(entity.Transform, entity.MovementTarget.Transform))
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

        private bool IsClockwiseMoving(Transform from, Transform to)
        {
            Vector3 directionFromFixedToCurrent = from.position - to.position;
            Vector3 normalVector = new Vector3(-directionFromFixedToCurrent.z, 0, directionFromFixedToCurrent.x);
            Vector3 movementVector = from.forward;
            float dotProduct = Vector3.Dot(normalVector, movementVector);
            return dotProduct < 0f;
        }
    }
}
