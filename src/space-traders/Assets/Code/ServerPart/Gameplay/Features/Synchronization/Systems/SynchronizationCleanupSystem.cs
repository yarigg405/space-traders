using Entitas;
using System.Collections.Generic;


namespace Assets.Code.ServerPart.Gameplay.Features.Synchronization.Systems
{
    internal sealed class SynchronizationCleanupSystem : ICleanupSystem
    {
        private readonly IGroup<GameEntity> _entities;
        private readonly List<GameEntity> _buffer = new(32);

        public SynchronizationCleanupSystem(GameContext game)
        {
            _entities = game.GetGroup(GameMatcher.NeedSynchronize);
        }

        void ICleanupSystem.Cleanup()
        {
            foreach (var entity in _entities.GetEntities(_buffer))
            {
                entity.isNeedSynchronize = false;
            }
        }
    }
}
