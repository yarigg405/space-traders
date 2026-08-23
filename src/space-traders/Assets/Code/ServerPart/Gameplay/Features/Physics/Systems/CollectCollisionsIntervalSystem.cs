using Assets.Code.Common;
using Entitas;


namespace Assets.Code.ServerPart.Gameplay.Features.Physics.Systems
{
    internal sealed class CollectCollisionsIntervalSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _entities;

        public CollectCollisionsIntervalSystem(GameContext game)
        {
            _entities = game.GetGroup(GameMatcher.AllOf(
                GameMatcher.CollectCollisionsTimer,
                GameMatcher.CollectCollisionsInterval
                ));
        }

        void IExecuteSystem.Execute()
        {
            foreach (var entity in _entities)
            {
                entity.ReplaceCollectCollisionsTimer(entity.CollectCollisionsTimer - GameConstants.FIXED_DELTA_TIME);

                if (entity.CollectCollisionsTimer <= 0)
                {
                    entity.isReadyToCollectCollisions = true;
                    entity.ReplaceCollectCollisionsTimer(entity.CollectCollisionsInterval);
                }
            }
        }
    }
}
