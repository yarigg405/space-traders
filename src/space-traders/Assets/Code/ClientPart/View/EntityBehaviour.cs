using UnityEngine;


namespace Assets.Code.ClientPart.View
{
    public sealed class EntityBehaviour : MonoBehaviour
    {
        private GameEntity _entity;

        public GameEntity Entity => _entity;

        public void SetEntity(GameEntity entity)
        {
            _entity = entity;
            _entity.AddView(this);
            _entity.Retain(this);

            foreach (var registrar in GetComponentsInChildren<IEntityComponentRegistrar>())
            {
                registrar.RegisterComponents();
            }
        }

        public void ReleaseEntity()
        {
            foreach (var registrar in GetComponentsInChildren<IEntityComponentRegistrar>())
            {
                registrar.UnRegisterComponents();
            }
            _entity.Release(this);
            _entity = null;
        }
    }
}
