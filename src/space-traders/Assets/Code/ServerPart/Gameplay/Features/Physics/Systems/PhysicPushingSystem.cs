using Assets.Code.Common;
using Assets.Code.ServerPart.Worlds.GameSynchronization;
using Entitas;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;


namespace Assets.Code.ServerPart.Gameplay.Features.Physics.Systems
{
    internal sealed class PhysicPushingSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _entities;
        private readonly List<GameEntity> _buffer = new(128);
        private readonly EntitiesSynchronizator _synchronizator;

        public PhysicPushingSystem(GameContext game, EntitiesSynchronizator synchronizator)
        {
            _entities = game.GetGroup(GameMatcher.AllOf(
               GameMatcher.PhysicsRadius,
               GameMatcher.GlobalPosition)
               .NoneOf(GameMatcher.Trigger));
            _synchronizator = synchronizator;
        }

        void IExecuteSystem.Execute()
        {
            _entities.GetEntities(_buffer);
            int count = _buffer.Count;

            for (int i = 0; i < count; i++)
            {
                var entityA = _buffer[i];

                var quadrantA = entityA.QuadrantIndex;
                var globalPosA = entityA.GlobalPosition;
                var radiusA = entityA.PhysicsRadius;
                var velocityA = entityA.Velocity;

                for (int j = i + 1; j < count; j++)
                {
                    var entityB = _buffer[j];

                    var quadrantB = entityB.QuadrantIndex;
                    var globalPosB = entityB.GlobalPosition;
                    var radiusB = entityB.PhysicsRadius;
                    var velocityB = entityB.Velocity;

                    var dx = quadrantA.x - quadrantB.x;
                    if (dx > 1 || dx < -1)
                        continue;

                    var dy = quadrantA.y - quadrantB.y;
                    if (dy > 1 || dy < -1)
                        continue;

                    var deltaVector = globalPosA - globalPosB;

                    var currentDistance = deltaVector.SqrMagnitude();
                    var minDistance = radiusA + radiusB;
                    if (currentDistance > minDistance * minDistance)
                        continue;


                    /// Collision and pushing calculations
                     
                    var dist = math.sqrt(currentDistance);
                    var penetration = minDistance - dist;
                    var normal = deltaVector / dist;

                    var totalMass = entityA.Mass + entityB.Mass;
                    var massFactorA = entityA.Mass / totalMass;
                    var massFactorB = entityB.Mass / totalMass;

                    var speedA = velocityA.magnitude;
                    var speedB = velocityB.magnitude;

                    var deltaDirection = new Vector2((float)normal.x, (float)normal.y);
                    var scalarProduct = Vector2.Dot(deltaDirection, velocityB.normalized);

                    var angle = Mathf.Acos(Mathf.Clamp(scalarProduct, -1f, 1f)) * Mathf.Rad2Deg;

                    entityA.ReplaceVelocity(velocityA + deltaDirection * speedB * massFactorA);
                    entityB.ReplaceVelocity(velocityB - deltaDirection * speedA * massFactorB);

                    _synchronizator.UpdateComponentsForEntity(entityA, GameComponentsLookup.Velocity);
                    _synchronizator.UpdateComponentsForEntity(entityB, GameComponentsLookup.Velocity);
                }
            }
        }
    }
}
