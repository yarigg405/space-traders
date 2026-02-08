using Assets.Code.Common;
using Assets.Code.ServerPart.Gameplay.Features.Physics.Triggers;
using Entitas;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Code.ServerPart.Gameplay.Features.Physics.Systems
{
    internal sealed class CastForTriggersInteractionsSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _parentEntitiesWithTriggers;
        private readonly List<GameEntity> _parentEntitiesBuffer = new List<GameEntity>(32);

        private readonly IGroup<GameEntity> _triggers;
        private readonly List<GameEntity> _triggersBuffer = new(64);

        private readonly IGroup<GameEntity> _entities;

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
            foreach (var parentEntity in _parentEntitiesWithTriggers.GetEntities(_parentEntitiesBuffer))
            {
                foreach (var entity in _entities)
                {
                    var quadrantDelta = parentEntity.QuadrantIndex - entity.QuadrantIndex;
                    if (Mathf.Abs(quadrantDelta.x) > 1 ||
                        Mathf.Abs(quadrantDelta.y) > 1)
                        continue;

                    foreach (var child in parentEntity.ChildrenEntities)
                    {
                        var deltaVector = child.GlobalPosition - entity.GlobalPosition;
                        var distance = deltaVector.SqrMagnitude();
                        var minDistance = child.PhysicsRadius + entity.PhysicsRadius;

                        if (distance <= minDistance * minDistance)
                        {
                            child.CollisionsBuffer.Add(entity.Id);
                        }

                        _interactionsService.UpdateInteractions(child.Id, child.CollisionsBuffer);
                    }
                }

                parentEntity.isReadyToCollectCollisions = false;
            }
        }

        private void HandleSingleTriggerEntities()
        {
            foreach (var trigger in _triggers.GetEntities(_triggersBuffer))
            {
                foreach (var entity in _entities)
                {
                    var quadrantDelta = trigger.QuadrantIndex - entity.QuadrantIndex;
                    if (Mathf.Abs(quadrantDelta.x) > 1 ||
                        Mathf.Abs(quadrantDelta.y) > 1)
                        continue;

                    var deltaVector = trigger.GlobalPosition - entity.GlobalPosition;
                    var distance = deltaVector.SqrMagnitude();
                    var minDistance = trigger.PhysicsRadius + entity.PhysicsRadius;

                    if (distance <= minDistance * minDistance)
                    {
                        trigger.CollisionsBuffer.Add(entity.Id);
                        _interactionsService.UpdateInteractions(trigger.Id, trigger.CollisionsBuffer);
                    }
                }
                trigger.isReadyToCollectCollisions = false;
            }
        }
    }
}
