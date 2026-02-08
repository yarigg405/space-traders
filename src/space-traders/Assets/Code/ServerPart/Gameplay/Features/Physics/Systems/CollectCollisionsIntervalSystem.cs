using Assets.Code.Common.Time;
using Entitas;


namespace Assets.Code.ServerPart.Gameplay.Features.Physics.Systems
{
    internal sealed class CollectCollisionsIntervalSystem : IExecuteSystem
    {
        private readonly ITimeService _time;
        private readonly IGroup<GameEntity> _entities;

        public CollectCollisionsIntervalSystem(GameContext game, ITimeService time)
        {
            _time = time;
            _entities = game.GetGroup(GameMatcher.AllOf(
                GameMatcher.CollectCollisionsTimer,
                GameMatcher.CollectCollisionsInterval
                ));
        }

        void IExecuteSystem.Execute()
        {
            foreach (var entity in _entities)
            {
                entity.ReplaceCollectCollisionsTimer(entity.CollectCollisionsTimer - _time.DeltaTime);

                if (entity.CollectCollisionsTimer <= 0)
                {
                    entity.isReadyToCollectCollisions = true;
                    entity.ReplaceCollectCollisionsTimer(entity.CollectCollisionsInterval);
                }
            }
        }
    }
}
