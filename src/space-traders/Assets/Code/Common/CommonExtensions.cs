using Entitas;
using System;
using Unity.Mathematics;
using UnityEngine;
using Yrr.Utils;
using Random = UnityEngine.Random;


namespace Assets.Code.Common
{
    public static class CommonExtensions
    {
        private const double _astronomicUnitLength = 149_597_870_700;
        private const double _astronomicUnitMinThreshold = _astronomicUnitLength * 0.1;

        public static float GetDirectionAngleFromTo(double2 fromPosition, double2 toPosition)
        {
            var direction = toPosition - fromPosition;
            var angleRad = Math.Atan2(direction.x, direction.y);
            var angleDeg = angleRad * Mathf.Rad2Deg;
            return AnglesUtil.NormalizeAngle((float)angleDeg);
        }

        public static double DoubleLerp(double start, double end, double t)
        {
            return start + (end - start) * t;
        }

        public static double2 ToDouble2(this Vector2 vector)
        {
            return new double2(vector.x, vector.y);
        }

        public static double2 ToDouble2XZ(this Vector3 vector)
        {
            return new double2(vector.x, vector.z);
        }

        public static string ToDistanceText(this double distance)
        {
            if (distance < 1000)
            {
                return $"{distance.ToString("###")}m";
            }

            if (distance < _astronomicUnitMinThreshold)
            {
                return $"{distance.ToShortMoneyString()} km";
            }

            var countOfUnits = distance / _astronomicUnitLength;
            return $"{countOfUnits.ToShortMoneyString()} au";
        }

        public static double2 GetRandomCoordinatesAroundPointZX(this double2 originalPoint, float radius,
                bool pointOnRadiusLine = false)
        {
            double angle = Random.Range(0, 360);
            var length = pointOnRadiusLine ? radius :
                Random.Range(0, radius);

            var x = math.cos(angle * Mathf.Deg2Rad) * length;
            var y = math.sin(angle * Mathf.Deg2Rad) * length;

            return new double2(originalPoint.x + x, originalPoint.y + y);
        }

        public static double2 MoveTowards(double2 current, double2 target, double maxDistanceDelta)
        {
            var num = target.x - current.x;
            var num2 = target.y - current.y;
            var num3 = num * num + num2 * num2;

            if (num3 == 0f || (maxDistanceDelta >= 0f && num3 <= maxDistanceDelta * maxDistanceDelta))
            {
                return target;
            }

            float num4 = (float)Math.Sqrt(num3);
            return new double2(current.x + num / num4 * maxDistanceDelta, current.y + num2 / num4 * maxDistanceDelta);
        }

        public static double Magnitude(this double2 vector)
        {
            return math.length(vector);
        }

        public static double SqrMagnitude(this double2 vector)
        {
            return math.lengthsq(vector);
        }

        public static int2 ToQuadrantIndex(this double2 globalPosition)
        {
            var x = globalPosition.x;
            var y = globalPosition.y;

            int quadrantX = (int)math.floor((x + GameConstants.GAME_SCENE_HALF_QUADRANT_SIZE) / GameConstants.GAME_SCENE_QUADRANT_SIZE);
            int quadrantY = (int)math.floor((y + GameConstants.GAME_SCENE_HALF_QUADRANT_SIZE) / GameConstants.GAME_SCENE_QUADRANT_SIZE);

            return new(quadrantX, quadrantY);
        }
    }
}
