using Assets.Code.Gameplay.Common.Time;
using Entitas;
using System.Collections.Generic;


namespace Assets.Code.Common.Destruct.Systems
{
    internal sealed class SelfDestructTimerSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _entities;
        private readonly ITimeService _time;
        private readonly List<GameEntity> _buffer = new(32);

        internal SelfDestructTimerSystem(GameContext game, ITimeService time)
        {
            _entities = game.GetGroup(GameMatcher.SelfDestructTimer);
            _time = time;
        }

        void IExecuteSystem.Execute()
        {
            foreach (var entity in _entities.GetEntities(_buffer))
            {
                var timer = entity.SelfDestructTimer - _time.DeltaTime;
                entity.ReplaceSelfDestructTimer(timer);

                if (timer <= 0)
                {
                    entity.RemoveSelfDestructTimer();
                    entity.isDestructed = true;
                }
            }
        }
    }
}