using System.Collections.Generic;
using Unity.Mathematics;


namespace Assets.Code.Common.Physics.Services
{
    public sealed class PhysicsService : IPhysicsService, IPhysicsRegistrar
    {
        private readonly Dictionary<uint, int2> _quadrantIndexes = new();
        private readonly Dictionary<int2, List<uint>> _entitiesInQuadrants = new();



        void IPhysicsRegistrar.RefreshPositionFor(GameEntity entity)
        {
            if (!_quadrantIndexes.ContainsKey(entity.Id))
            {
                _quadrantIndexes.Add(entity.Id, entity.QuadrantIndex);
                if (!_entitiesInQuadrants.ContainsKey(entity.QuadrantIndex))
                    _entitiesInQuadrants[entity.QuadrantIndex] = new();

                _entitiesInQuadrants[entity.QuadrantIndex].Add(entity.Id);
                return;
            }

            var previousIndex = _quadrantIndexes[entity.Id];
            if (previousIndex.Equals(entity.QuadrantIndex)) return;

            _entitiesInQuadrants[previousIndex].Remove(entity.Id);
            _quadrantIndexes[entity.Id] = entity.QuadrantIndex;

            if (!_entitiesInQuadrants.ContainsKey(entity.QuadrantIndex))
                _entitiesInQuadrants[entity.QuadrantIndex] = new();

            _entitiesInQuadrants[entity.QuadrantIndex].Add(entity.Id);
        }

        void IPhysicsRegistrar.RemoveEntity(uint entityId)
        {
            if (!_quadrantIndexes.ContainsKey(entityId)) return;

            var index = _quadrantIndexes[entityId];
            _entitiesInQuadrants[index].Remove(entityId);
            _quadrantIndexes.Remove(entityId);
        }
    }
}
