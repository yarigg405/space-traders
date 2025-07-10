using Entitas;


namespace Assets.Code.Gameplay.Features.TargetCollection.Systems
{
    internal sealed class CleanupTargetBuffersSystem : ICleanupSystem
    {
        private readonly IGroup<GameEntity> _entities;

        internal CleanupTargetBuffersSystem(GameContext game)
        {
            _entities = game.GetGroup(GameMatcher.AllOf(
                GameMatcher.TargetsBuffer
            ));
        }

        void ICleanupSystem.Cleanup()
        {
            foreach (var entity in _entities)
            {
                entity.TargetsBuffer.Clear();
            }
        }
    }
}