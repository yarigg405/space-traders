using Assets.Code.Common.Extensions;
using Assets.Code.Common.Serialization;
using Assets.Code.Common.Serialization.Data;
using Assets.Code.Common.Serialization.Extensions;
using Unity.Mathematics;
using UnityEngine;
using Yrr.Utils;


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
                .AddCurrentRotationY(0)
                .AddVelocity(Vector2.zero)
                .AddCurrentMoveSpeed(0)

                .FillEntityWith(snapshot)
                ;
        }

        public void DestroyEntity(uint entityId)
        {
            var destroyEntity = _gameContext.GetEntityWithId(entityId);
            if (destroyEntity == null) return;

            destroyEntity.With(x => x.isDestructed = true);
        }


        // Update values

        public void UpdateGlobalPosition(uint entityId, double2 serverGlobalPosition)
        {
            var entity = _gameContext.GetEntityWithId(entityId);
            if (entity == null) return;
            if (entity.isClientPlayer) return;

            var delta = serverGlobalPosition - entity.GlobalPosition;

            if (math.abs(delta.x) > 5 ||
                math.abs(delta.y) > 5)
                entity.ReplaceGlobalPosition(serverGlobalPosition);
        }

        public void UpdateRotation(uint entityId, float serverRotation)
        {
            var entity = _gameContext.GetEntityWithId(entityId);
            if (entity == null) return;
            if (entity.isClientPlayer) return;

            var current = entity.CurrentRotationY;

            if (AnglesUtil.GetMinAngledDelta(current, serverRotation) > 15)
                entity.ReplaceCurrentRotationY(serverRotation);
        }

        public void UpdateEntityComponents(uint entityId, EntitySnapshot snapshot)
        {
            var entity = _gameContext.GetEntityWithId(entityId);
            if (entity == null) return;
            if (entity.isClientPlayer) return;

            foreach (var component in snapshot.ComponentsForRemoving)
            {
                if (entity.HasComponent(component))
                    entity.RemoveComponent(component);
            }

            foreach (var component in snapshot.Components)
            {
                var lookupIndex = ComponentIndexByType.IndexByType(component.GetType());
                entity.With(x => x.ReplaceComponent(lookupIndex, component), when: lookupIndex >= 0);
            }
        }
    }
}
