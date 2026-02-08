using Entitas;


namespace Assets.Code.ServerPart.Gameplay.Features.Physics.Systems
{
    internal sealed class CleanupCollisionsBufferSystem : ICleanupSystem
    {
        private IGroup<GameEntity> _entities;

        public CleanupCollisionsBufferSystem(GameContext game)
        {
            _entities = game.GetGroup(GameMatcher.CollisionsBuffer);
        }

        void ICleanupSystem.Cleanup()
        {
            foreach (var entity in _entities)
            {
                entity.CollisionsBuffer.Clear();
            }
        }
    }
}
