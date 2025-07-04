using UnityEngine;


namespace Yrr.Utils
{
    public static class AnglesUtil
    {
        public static float GetAngleDirectionY(Vector3 fromPosition, Vector3 toPosition)
        {
            Vector3 direction = toPosition - fromPosition;
            float angleRad = Mathf.Atan2(direction.x, direction.z);
            float angleDeg = angleRad * Mathf.Rad2Deg;
            return NormalizeAngle(angleDeg);
        }

        public static float NormalizeAngle(float angle)
        {
            while (angle >= 360f) angle -= 360f;
            while (angle < 0f) angle += 360f;
            return angle;
        }

        public static float MoveTowardsAngle(this float currentAngle, float targetAngle, float maxDeltaRotation)
        {
            var diff = Mathf.DeltaAngle(currentAngle, targetAngle);
            var move = Mathf.Clamp(diff, -maxDeltaRotation, maxDeltaRotation);
            return NormalizeAngle(currentAngle + move);
        }

        public static float GetDirectionAngleFromTo(Vector3 fromPosition, Vector3 toPosition)
        {
            Vector3 direction = toPosition - fromPosition;
            float angleRad = Mathf.Atan2(direction.x, direction.z);
            float angleDeg = angleRad * Mathf.Rad2Deg;
            return NormalizeAngle(angleDeg);
        }
    }
}
