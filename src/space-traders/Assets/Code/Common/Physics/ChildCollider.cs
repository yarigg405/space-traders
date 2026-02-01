using Unity.Mathematics;


namespace Assets.Code.Common.Physics
{
    public readonly struct ChildCollider
    {
        public readonly double2 GlobalPosition;
        public readonly float Radius;
        public readonly int ColliderIndex;

        public ChildCollider(double2 globalPosition, float radius, int colliderIndex)
        {
            GlobalPosition = globalPosition;
            Radius = radius;
            ColliderIndex = colliderIndex;
        }
    }
}
