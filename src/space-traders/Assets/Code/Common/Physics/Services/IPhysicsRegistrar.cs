namespace Assets.Code.Common.Physics.Services
{
    public interface IPhysicsRegistrar
    {
        void RefreshPositionFor(GameEntity entity);
        void RemoveEntity(uint entityId);
    }
}
