using Assets.Code.Common;
using Entitas;
using System.Collections.Generic;


namespace Assets.Code.ServerPart.Gameplay.Features.Destruct.Systems
{
    internal sealed class SelfDestructTimerSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _entities;
        private readonly List<GameEntity> _buffer = new(32);

        internal SelfDestructTimerSystem(GameContext game)
        {
            _entities = game.GetGroup(GameMatcher.SelfDestructTimer);
        }

        void IExecuteSystem.Execute()
        {
            foreach (var entity in _entities.GetEntities(_buffer))
            {
                var timer = entity.SelfDestructTimer - GameConstants.FIXED_DELTA_TIME;
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