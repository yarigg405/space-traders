using System;
using UnityEngine;
using Yrr.Utils;


namespace Assets.Code.Gameplay.Common
{
    public static class CommonExtensions
    {
        public static float GetDirectionAngleFromTo(Vector2Double fromPosition, Vector2Double toPosition)
        {
            var direction = toPosition - fromPosition;
            var angleRad = Math.Atan2(direction.X, direction.Y);
            var angleDeg = angleRad * Mathf.Rad2Deg;
            return AnglesUtil.NormalizeAngle((float)angleDeg);
        }
    }
}
