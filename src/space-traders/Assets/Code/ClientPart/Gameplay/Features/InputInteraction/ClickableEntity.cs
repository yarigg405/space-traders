using Assets.Code.ClientPart.View;
using UnityEngine;


namespace Assets.Code.ClientPart.Gameplay.Features.InputInteraction
{
    public class ClickableEntity : MonoBehaviour
    {
        [SerializeField] private EntityBehaviour _entityBehaviour;
        public GameEntity Entity => _entityBehaviour.Entity;

        private void OnValidate()
        {
            if (!_entityBehaviour)
                _entityBehaviour = GetComponent<EntityBehaviour>();
        }
    }
}