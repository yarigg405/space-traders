using Assets.Code.Common;
using Assets.Code.ServerPart.Worlds.GameSynchronization;
using Entitas;
using System.Collections.Generic;


namespace Assets.Code.ServerPart.Gameplay.Features.Movement.Systems
{
    internal sealed class ShipUndockTimerSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _entities;
        private readonly List<GameEntity> _buffer = new(32);

        private readonly EntitiesSynchronizator _synchronizator;


        public ShipUndockTimerSystem(GameContext game, EntitiesSynchronizator synchronizator)
        {
            _entities = game.GetGroup(GameMatcher.UndockingTimer);
            _synchronizator = synchronizator;
        }

        void IExecuteSystem.Execute()
        {
            foreach (var entity in _entities.GetEntities(_buffer))
            {
                var time = entity.UndockingTimer - GameConstants.FIXED_DELTA_TIME;
                entity.ReplaceUndockingTimer(time);

                if (time <= 0)
                {
                    entity.FinishUndocking();
                    _synchronizator.UpdateComponentsForEntity(entity,
                        GameComponentsLookup.Moving,
                        GameComponentsLookup.IgnoreCollision,
                        GameComponentsLookup.UndockingInProcess,
                        GameComponentsLookup.DockingInProcess
                        );
                }
            }
        }
    }
}
