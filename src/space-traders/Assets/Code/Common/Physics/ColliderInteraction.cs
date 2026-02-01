namespace Assets.Code.Common.Physics
{
    public readonly struct ColliderInteraction
    {
        public readonly uint CollidingEntity;
        public readonly int ColliderIndex;

        public ColliderInteraction(uint collidingEntity, int colliderIndex)
        {
            CollidingEntity = collidingEntity;
            ColliderIndex = colliderIndex;
        }
    }
}
