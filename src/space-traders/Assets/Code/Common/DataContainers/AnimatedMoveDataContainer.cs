using UnityEngine;


namespace Assets.Code.Common.DataContainers
{
    public readonly struct AnimatedMoveDataContainer
    {
        public readonly Vector3 From; 
        public readonly Vector3 To;
        public readonly float MaxTime;

        public AnimatedMoveDataContainer(Vector3 from, Vector3 to, float maxTime) : this()
        {
            From = from;
            To = to;
            MaxTime = maxTime;
        }
    }
}
