using System;
using Unity.Mathematics;


namespace Assets.Code.Common.DataContainers
{
    [Serializable]
    public sealed class WarpDataContainer
    {
        public double2 WarpStartPosition;
        public double2 WarpFinishPosition;
        public double WarpTotalDistance;

        public double WarpSpeed;
        public double TopSpeed;
        public double Acceleration;
        public double TopDistance;

        public WarpDataContainer(double2 startPosition, double2 finishPosition)
        {
            WarpStartPosition = startPosition;
            WarpFinishPosition = finishPosition;
            WarpTotalDistance = math.length(WarpFinishPosition - WarpStartPosition);

            WarpSpeed = 0;
        }
    }
}
