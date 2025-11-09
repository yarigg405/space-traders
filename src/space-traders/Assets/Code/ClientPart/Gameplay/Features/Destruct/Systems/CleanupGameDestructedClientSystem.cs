using Entitas;
using System.Collections.Generic;


namespace Assets.Code.ClientPart.Gameplay.Features.Destruct.Systems
{
    internal sealed class CleanupGameDestructedClientSystem : ICleanupSystem
    {
        private readonly IGroup<GameEntity> _entities;
        private readonly List<GameEntity> _buffer = new(32);

        internal CleanupGameDestructedClientSystem(GameContext game)
        {
            _entities = game.GetGroup(GameMatcher.Destructed);
        }

        void ICleanupSystem.Cleanup()
        {
            foreach (var entity in _entities.GetEntities(_buffer))
            {
                entity.Destroy();
            }
        }
    }
}