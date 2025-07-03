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
    }
}
