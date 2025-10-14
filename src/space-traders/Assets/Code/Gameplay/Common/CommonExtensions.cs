using System;
using Unity.Mathematics;
using UnityEngine;
using Yrr.Utils;
using Random = UnityEngine.Random;


namespace Assets.Code.Gameplay.Common
{
    public static class CommonExtensions
    {
        private const double _astronomicUnitLenght = 149_597_870_700;
        private const double _astronomicUnitMinThreshold = _astronomicUnitLenght * 0.1;

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

            var countOfUnits = distance / _astronomicUnitLenght;
            return $"{countOfUnits.ToShortMoneyString()} au";
        }

        public static double2 GetRandomCoordinatesAroundPointZX(this double2 originalPoint, float radius,
                bool pointOnRadiusLine = false)
        {
            double angle = Random.Range(0, 360);
            var lenght = pointOnRadiusLine ? radius :
                (Random.Range(0, radius));

            var x = math.cos(angle * Mathf.Deg2Rad) * lenght;
            var y = math.sin(angle * Mathf.Deg2Rad) * lenght;

            return new double2(originalPoint.x + x, originalPoint.y + y);
        }
    }
}
