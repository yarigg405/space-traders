using Assets.Code.Common.Time;
using Assets.Code.ServerPart.Worlds.GameSynchronization;
using Entitas;
using System.Collections.Generic;


namespace Assets.Code.ServerPart.Gameplay.Features.Movement.Systems
{
    internal sealed class ShipUndockTimerSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _entities;
        private readonly List<GameEntity> _buffer = new(32);

        private readonly ITimeService _time;
        private readonly EntitiesSynchronizator _synchronizator;


        public ShipUndockTimerSystem(GameContext game,
            EntitiesSynchronizator synchronizator, ITimeService time)
        {
            _entities = game.GetGroup(GameMatcher.UndockingTimer);
            _synchronizator = synchronizator;
            _time = time;
        }

        void IExecuteSystem.Execute()
        {
            foreach (var entity in _entities.GetEntities(_buffer))
            {
                var time = entity.UndockingTimer - _time.DeltaTime;
                entity.ReplaceUndockingTimer(time);

                if (time <= 0)
                {
                    entity.FinishUndocking();
                    _synchronizator.UpdateComponentsForEntity(entity,
                        GameComponentsLookup.Moving,
                        GameComponentsLookup.IgnoreCollision,
                        GameComponentsLookup.UndockingInProccess,
                        GameComponentsLookup.DockingInProccess
                        );
                }
            }
        }
    }
}
