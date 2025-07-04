using Assets.Code.Gameplay.Common.Collisions;
using UnityEngine;
using VContainer;


namespace Assets.Code.View
{
    public sealed class EntityBehaviour : MonoBehaviour
    {
        private GameEntity _entity;
        private ICollisionRegistry _collisionRegistry;

        public GameEntity Entity => _entity;

        [Inject]
        private void Construct(ICollisionRegistry collisionRegistry)
        {
            _collisionRegistry = collisionRegistry;
        }

        public void SetEntity(GameEntity entity)
        {
            _entity = entity;
            _entity.AddView(this);
            _entity.Retain(this);

            foreach (var registrar in GetComponentsInChildren<IEntityComponentRegistrar>())
            {
                registrar.RegisterComponents();
            }

            foreach (var collider in GetComponentsInChildren<Collider>(true))
            {
                _collisionRegistry.Register(collider.GetInstanceID(), _entity);
            }
        }

        public void ReleaseEntity()
        {
            foreach (var collider in GetComponentsInChildren<Collider>(true))
            {
                _collisionRegistry.UnRegister(collider.GetInstanceID());
            }

            foreach (var registrar in GetComponentsInChildren<IEntityComponentRegistrar>())
            {
                registrar.UnRegisterComponents();
            }
            _entity.Release(this);
            _entity = null;
        }
    }
}
