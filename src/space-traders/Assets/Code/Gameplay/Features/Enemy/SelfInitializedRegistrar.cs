using Assets.Code.Common.Entity;
using Assets.Code.View;
using UnityEngine;


namespace Assets.Code.Gameplay.Features.Enemy
{
    internal sealed class SelfInitializedRegistrar : MonoBehaviour
    {
        [SerializeField] private EntityBehaviour _playerEntity;
        [SerializeField] private EntityBehaviour _enemyEntity;


        private void Start()
        {
            _playerEntity.SetEntity(CreateEntity.Empty());

            _enemyEntity.SetEntity(CreateEntity.Empty());
        }
    }
}
