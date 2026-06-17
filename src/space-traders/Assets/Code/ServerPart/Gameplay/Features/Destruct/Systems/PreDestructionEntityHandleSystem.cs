using Assets.Code.ServerPart.Gameplay.Features.Physics.Triggers;
using Entitas;
using System.Collections.Generic;


namespace Assets.Code.ServerPart.Gameplay.Features.Destruct.Systems
{
    internal sealed class PreDestructionEntityHandleSystem : ICleanupSystem
    {
        private readonly TriggersInteractionsService _triggersInteractionsService;

        private readonly IGroup<GameEntity> _entities;
        private readonly List<GameEntity> _buffer = new(32);

        public PreDestructionEntityHandleSystem(GameContext game,
            TriggersInteractionsService triggersInteractionsService)
        {
            _entities = game.GetGroup(
                GameMatcher
                .AllOf(GameMatcher.Destructed)
                .NoneOf(GameMatcher.Disposed));

            _triggersInteractionsService = triggersInteractionsService;
        }

        void ICleanupSystem.Cleanup()
        {
            foreach (var entity in _entities.GetEntities(_buffer))
            {
                _triggersInteractionsService.RemoveEntity(entity.Id);
                entity.isDisposed = true;
            }
        }
    }
}
