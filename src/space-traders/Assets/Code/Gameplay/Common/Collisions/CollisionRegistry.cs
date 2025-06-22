using Entitas;
using System.Collections.Generic;


namespace Assets.Code.Gameplay.Common.Collisions
{
    internal sealed class CollisionRegistry : ICollisionRegistry
    {
        private readonly Dictionary<int, IEntity> _entityByInstanceId = new();

        void ICollisionRegistry.Register(int instanceId, IEntity entity)
        {
            _entityByInstanceId[instanceId] = entity;
        }

        void ICollisionRegistry.UnRegister(int instanceId)
        {
            if (_entityByInstanceId.ContainsKey(instanceId))
                _entityByInstanceId.Remove(instanceId);
        }

        TEntity ICollisionRegistry.Get<TEntity>(int instanceId)
        {
            if (_entityByInstanceId.TryGetValue(instanceId, out var entity))
                return entity as TEntity;

            return null;
        }
    }
}
