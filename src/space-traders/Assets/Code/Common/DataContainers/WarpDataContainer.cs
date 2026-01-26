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

        public double WarpMaxSpeed;
        public float WarpAccelerationMaxTime;

        public double WarpSpeedCurrent;
        public float CurrentWarpingTime;

        public int WarpGear;

        public WarpDataContainer(double2 startPosition, double2 finishPosition)
        {
            WarpStartPosition = startPosition;
            WarpFinishPosition = finishPosition;
            WarpTotalDistance = math.length(WarpFinishPosition - WarpStartPosition);

            WarpGear = 1;
        }
    }
}
