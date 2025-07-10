using Assets.Code.Gameplay.Common.Physics;
using Entitas;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Assets.Code.Gameplay.Features.TargetCollection.Systems
{
    internal sealed class CastForTargetsSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _entities;
        private readonly IPhysicsService _physicsService;
        private readonly LayerMask _targetsLayerMask = LayerMask.GetMask("Defaul");
        private readonly List<GameEntity> _buffer = new(64);

        internal CastForTargetsSystem(GameContext game, IPhysicsService physicsService)
        {
            _entities = game.GetGroup(GameMatcher.AllOf(
                GameMatcher.Transform,
                GameMatcher.TargetsBuffer,
                GameMatcher.Radius,
                GameMatcher.ReadyToCollectTargets
            ));
            _physicsService = physicsService;
        }

        void IExecuteSystem.Execute()
        {
            foreach (var entity in _entities.GetEntities(_buffer))
            {
                entity.TargetsBuffer.AddRange(TargetsInRadius(entity));
                entity.isReadyToCollectTargets = false;
            }
        }

        private IEnumerable<int> TargetsInRadius(GameEntity entity)
        {
            return _physicsService.SphereCast(entity.Transform.position, entity.Radius, _targetsLayerMask).Select(x => x.Id);
        }
    }
}