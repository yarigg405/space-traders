using Assets.Code.Common.Entity;
using Assets.Code.Infrastructure.Identifiers;
using UnityEngine;
using VContainer;


namespace Assets.Code.View
{
    internal sealed class SelfInitializedEntity : MonoBehaviour
    {
        [SerializeField] private EntityBehaviour _entityView;

        private IIdentifierService _identifierService;

        [Inject]
        private void Contruct(IIdentifierService identifierService)
        {
            _identifierService = identifierService;

            gameObject.SetActive(true);
        }

        private void OnValidate()
        {
            if (!_entityView)
                _entityView = GetComponent<EntityBehaviour>();
        }

        private void Awake()
        {
            var entity = CreateEntity.Empty().AddId(_identifierService.Next());
            _entityView.SetEntity(entity);
        }
    }
}
