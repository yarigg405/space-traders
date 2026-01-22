using Assets.Code.Common.Extensions;
using Assets.Code.Common.Serialization.Data;
using Assets.Code.Common.Serialization.Extensions;
using Unity.Mathematics;

namespace Assets.Code.ClientPart
{
    public sealed class ClientEntitiesController
    {
        private readonly GameContext _gameContext;

        public ClientEntitiesController(GameContext gameContext)
        {
            _gameContext = gameContext;
        }

        public void CreateEntityFromSnapshot(EntitySnapshot snapshot)
        {
            _gameContext.CreateEntity()
                .FillEntityWith(snapshot);
        }

        public void DestroyEntity(uint entityId)
        {
            var destroyEntity = _gameContext.GetEntityWithId(entityId);
            if (destroyEntity == null) return;

            destroyEntity.With(x => x.isDestructed = true);
        }



        ///      Update values

        public void UpdateGlobalPosition(uint entityId, double2 newGlobalPosition)
        {
            var entity = _gameContext.GetEntityWithId(entityId);
            if (entity == null) return;

            entity.ReplaceGlobalPosition(newGlobalPosition);
        }

        public void UpdateRotation(uint entityId, float currentRotation)
        {
            var entity = _gameContext.GetEntityWithId(entityId);
            if (entity == null) return;

            entity.ReplaceCurrentRotationY(currentRotation);
        }
    }
}
