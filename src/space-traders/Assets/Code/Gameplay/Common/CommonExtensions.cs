using System;
using Unity.Mathematics;
using UnityEngine;
using Yrr.Utils;


namespace Assets.Code.Gameplay.Common
{
    public static class CommonExtensions
    {
        private const double _astronomicUnitLenght = 149_597_870_700;
        private const double _astronomicUnitMinChreshold = _astronomicUnitLenght * 0.1;

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

            if (distance < _astronomicUnitMinChreshold)
            {
                return $"{distance.ToShortMoneyString()} km";
            }

            var countOfUnits = distance / _astronomicUnitLenght;
            return $"{countOfUnits.ToShortMoneyString()} au";
        }
    }
}
