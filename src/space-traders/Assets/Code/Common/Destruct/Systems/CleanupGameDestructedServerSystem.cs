using Assets.Code.Gameplay.Worlds.GameSynchronization;
using Entitas;
using System.Collections.Generic;


namespace Assets.Code.Common.Destruct.Systems
{
    internal sealed class CleanupGameDestructedServerSystem : ICleanupSystem
    {
        private readonly IEntityDestroyer _entityDestroyer;

        private readonly IGroup<GameEntity> _entities;
        private readonly List<GameEntity> _buffer = new(32);

        internal CleanupGameDestructedServerSystem(GameContext game, IEntityDestroyer entityDestroyer)
        {
            _entities = game.GetGroup(GameMatcher.Destructed);
            _entityDestroyer = entityDestroyer;
        }

        void ICleanupSystem.Cleanup()
        {
            foreach (var entity in _entities.GetEntities(_buffer))
            {
                _entityDestroyer.DestroyEntityOnClients(entity.Id);
                entity.Destroy();
            }
        }
    }
}