using System;

namespace Assets.Code.Gameplay.Common
{
    [Serializable]
    public sealed class Vector2Double
    {
        public Vector2Double(double x, double y)
        {
            X = x; Y = y;
        }

        public double X;
        public double Y;

        public static Vector2Double operator +(Vector2Double left, Vector2Double right)
        {
            return new Vector2Double(left.X+right.X, left.Y+right.Y);
        }

        public static Vector2Double operator -(Vector2Double left, Vector2Double right)
        {
            return new Vector2Double(left.X - right.X, left.Y - right.Y);
        }
    }
}
