using UnityEngine;


namespace Assets.Code.View
{
    internal abstract class EntityComponentRegistrar : MonoBehaviour, IEntityComponentRegistrar
    {
        [SerializeField] private EntityBehaviour _entityView;
        public GameEntity Entity => _entityView ? _entityView.Entity : null;

        public abstract void RegisterComponents();
        public abstract void UnRegisterComponents();
    }
}
