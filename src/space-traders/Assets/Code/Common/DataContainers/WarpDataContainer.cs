using System;
using Unity.Mathematics;


namespace Assets.Code.Common.DataContainers
{
    [Serializable]
    public sealed class WarpDataContainer
    {
        public double2 WarpFinishPosition;
        public double WarpTotalDistance;

        public double WarpSpeedCurrent;
        public float CurrentWarpingTime;
        public float StartBrakingTime;

        public bool IsBraking;
        public double2 StartBrakingPos;

        public WarpDataContainer(double2 startPosition, double2 finishPosition)
        {
            WarpFinishPosition = finishPosition;
            WarpTotalDistance = math.length(finishPosition - startPosition);

            IsBraking = false;
        }
    }
}
