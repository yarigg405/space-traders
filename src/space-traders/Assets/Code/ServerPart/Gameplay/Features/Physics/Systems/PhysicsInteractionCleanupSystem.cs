using Entitas;


namespace Assets.Code.ServerPart.Gameplay.Features.Physics.Systems
{
    internal sealed class PhysicsInteractionCleanupSystem : ICleanupSystem
    {
        private readonly IGroup<GameEntity> _entities;

        public PhysicsInteractionCleanupSystem(GameContext game)
        {
            _entities = game.GetGroup(GameMatcher.CollidersBuffer);
        }

        void ICleanupSystem.Cleanup()
        {
            foreach (var entity in _entities)
            {
                entity.CollidersBuffer.Clear();
            }
        }
    }
}
