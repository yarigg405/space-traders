using UnityEngine;
using Yrr.Utils;


namespace Assets.Code.Common.Extensions
{
    public static class MovementFormulas
    {
        public static void UpdateMoveSpeed(GameEntity e)
        {
            var target = e.MaxMoveSpeed * e.CurrentSpeedModifier;
            var delta = e.MovingAcceleration * GameConstants.FIXED_DELTA_TIME;
            e.ReplaceCurrentMoveSpeed(Mathf.MoveTowards(e.CurrentMoveSpeed, target, delta));
        }

        public static void Rotate(GameEntity e)
        {
            var delta = Mathf.DeltaAngle(e.CurrentRotationY, e.TargetRotation);
            var speedFactor = Mathf.Abs(delta) * 0.01f;
            var eff = e.RotationSpeed * GameConstants.FIXED_DELTA_TIME * speedFactor;
            e.ReplaceCurrentRotationY(AnglesUtil.MoveTowardsAngle(e.CurrentRotationY, e.TargetRotation, eff));
        }

        public static void HandleVelocity(GameEntity e)
        {
            var rad = e.CurrentRotationY * Mathf.Deg2Rad;
            var targetVel = new Vector2(Mathf.Sin(rad), Mathf.Cos(rad)) * e.CurrentMoveSpeed;
            e.ReplaceVelocity(Vector3.MoveTowards(e.Velocity, targetVel, e.VelocityAgility * GameConstants.FIXED_DELTA_TIME));
        }

        public static void Move(GameEntity e)
        {
            var pos = e.GlobalPosition;
            pos.x += e.Velocity.x * GameConstants.FIXED_DELTA_TIME;
            pos.y += e.Velocity.y * GameConstants.FIXED_DELTA_TIME;
            e.ReplaceGlobalPosition(pos);
        }
    }
}
