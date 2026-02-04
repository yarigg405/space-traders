using System;
using Unity.Mathematics;


namespace Assets.Code.ServerPart.Physics.Data
{
    [Serializable]
    public struct PhysicsShape
    {
        public double2 LocalCenter;
        public float Radius;

        public PhysicsShape(double2 localCenter, float radius)
        {
            LocalCenter = localCenter;
            Radius = radius;
        }
    }
}
