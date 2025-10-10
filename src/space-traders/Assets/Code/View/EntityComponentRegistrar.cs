using UnityEngine;


namespace Assets.Code.View
{
    internal abstract class EntityComponentRegistrar : MonoBehaviour, IEntityComponentRegistrar
    {
        [SerializeField] private EntityBehaviour _entityView;
        public GameEntity Entity => _entityView ? _entityView.Entity : null;

        private void OnValidate()
        {
            _entityView = GetComponent<EntityBehaviour>();
        }

        public abstract void RegisterComponents();
        public abstract void UnRegisterComponents();
    }
}
