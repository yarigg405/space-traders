using Assets.Code.ServerPart.Gameplay.Features.Physics.Triggers;
using Entitas;
using System.Collections.Generic;


namespace Assets.Code.ServerPart.Gameplay.Features.Physics.Systems
{
    internal sealed class CastForTriggersInteractionsSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _parentEntitiesWithTriggers;
        private readonly List<GameEntity> _parentEntitiesBuffer = new List<GameEntity>(32);

        private readonly IGroup<GameEntity> _triggers;
        private readonly List<GameEntity> _triggersBuffer = new(64);

        private readonly IGroup<GameEntity> _entities;
        private readonly List<GameEntity> _entitiesBuffer = new(128);
        private readonly TriggersInteractionsService _interactionsService;

        public CastForTriggersInteractionsSystem(GameContext game, TriggersInteractionsService interactionsService)
        {
            _parentEntitiesWithTriggers = game.GetGroup(GameMatcher.AllOf(
                GameMatcher.ReadyToCollectCollisions,
                GameMatcher.ChildrenEntities,
                GameMatcher.GlobalPosition
                ));

            _triggers = game.GetGroup(GameMatcher.AllOf(
                GameMatcher.PhysicsRadius,
                GameMatcher.Trigger)
                .NoneOf(GameMatcher.ParentEntity));

            _entities = game.GetGroup(GameMatcher.AllOf(
                GameMatcher.PhysicsRadius,
                GameMatcher.GlobalPosition)
                .NoneOf(GameMatcher.Trigger));
            _interactionsService = interactionsService;
        }

        void IExecuteSystem.Execute()
        {
            HandleParentEntities();
            HandleSingleTriggerEntities();
        }

        private void HandleParentEntities()
        {
            _parentEntitiesWithTriggers.GetEntities(_parentEntitiesBuffer);
            _entities.GetEntities(_entitiesBuffer);

            int parentCount = _parentEntitiesBuffer.Count;
            int entityCount = _entitiesBuffer.Count;

            for (int p = 0; p < parentCount; p++)
            {
                var parent = _parentEntitiesBuffer[p];

                var parentQuadrant = parent.QuadrantIndex;
                var children = parent.ChildrenEntities;

                if (children == null || children.Count == 0)
                {
                    parent.isReadyToCollectCollisions = false;
                    continue;
                }

                for (int c = 0; c < children.Count; c++)
                    children[c].CollisionsBuffer.Clear();

                for (int i = 0; i < entityCount; i++)
                {
                    var entity = _entitiesBuffer[i];
                    var entityQuadrant = entity.QuadrantIndex;

                    var dx = parentQuadrant.x - entityQuadrant.x;
                    if (dx > 1 || dx < -1)
                        continue;

                    var dy = parentQuadrant.y - entityQuadrant.y;
                    if (dy > 1 || dy < -1)
                        continue;

                    var entityPos = entity.GlobalPosition;
                    var entityRadius = entity.PhysicsRadius;

                    for (int c = 0; c < children.Count; c++)
                    {
                        var child = children[c];

                        var delta = child.GlobalPosition - entityPos;

                        var minDistance = child.PhysicsRadius + entityRadius;
                        var minDistanceSq = minDistance * minDistance;

                        var distSq = delta.x * delta.x + delta.y * delta.y;

                        if (distSq <= minDistanceSq)
                        {
                            child.CollisionsBuffer.Add(entity.Id);
                        }
                    }
                }

                for (int c = 0; c < children.Count; c++)
                {
                    var child = children[c];

                    _interactionsService.UpdateInteractions(
                        child.Id,
                        child.CollisionsBuffer);
                }

                parent.isReadyToCollectCollisions = false;
            }
        }

        private void HandleSingleTriggerEntities()
        {
            _triggers.GetEntities(_triggersBuffer);
            _entities.GetEntities(_entitiesBuffer);

            int triggerCount = _triggersBuffer.Count;
            int entityCount = _entitiesBuffer.Count;

            for (int t = 0; t < triggerCount; t++)
            {
                var trigger = _triggersBuffer[t];

                trigger.CollisionsBuffer.Clear();

                var triggerQuadrant = trigger.QuadrantIndex;
                var triggerPos = trigger.GlobalPosition;
                var triggerRadius = trigger.PhysicsRadius;

                for (int i = 0; i < entityCount; i++)
                {
                    var entity = _entitiesBuffer[i];

                    var entityQuadrant = entity.QuadrantIndex;

                    int dx = triggerQuadrant.x - entityQuadrant.x;
                    if (dx > 1 || dx < -1)
                        continue;

                    int dy = triggerQuadrant.y - entityQuadrant.y;
                    if (dy > 1 || dy < -1)
                        continue;

                    var entityPos = entity.GlobalPosition;
                    var entityRadius = entity.PhysicsRadius;

                    var minDistance = triggerRadius + entityRadius;
                    var minDistanceSq = minDistance * minDistance;

                    var delta = triggerPos - entityPos;

                    var distSq = delta.x * delta.x + delta.y * delta.y;

                    if (distSq <= minDistanceSq)
                    {
                        trigger.CollisionsBuffer.Add(entity.Id);
                    }
                }

                _interactionsService.UpdateInteractions(
                    trigger.Id,
                    trigger.CollisionsBuffer);

                trigger.isReadyToCollectCollisions = false;
            }
        }
    }
}
