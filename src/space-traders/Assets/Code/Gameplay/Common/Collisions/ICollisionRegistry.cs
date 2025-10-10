using Entitas;


namespace Assets.Code.Gameplay.Common.Collisions
{
    internal interface ICollisionRegistry
    {
        void Register(int instanceId, IEntity entity);
        void UnRegister(int instanceId);
        TEntity Get<TEntity>(int instanceId) where TEntity : class;
    }
}
