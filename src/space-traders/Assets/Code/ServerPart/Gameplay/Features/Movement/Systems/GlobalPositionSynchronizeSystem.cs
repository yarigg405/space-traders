using Assets.Code.ServerPart.Worlds.GameSynchronization;
using Entitas;


namespace Assets.Code.ServerPart.Gameplay.Features.Movement.Systems
{
    internal sealed class GlobalPositionSynchronizeSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _entities;
        private readonly EntitiesSyncronizator _syncronizator;

        internal GlobalPositionSynchronizeSystem(GameContext game, EntitiesSyncronizator syncronizator)
        {
            _entities = game.GetGroup(GameMatcher.GlobalPosition);
            _syncronizator = syncronizator;
        }

        void IExecuteSystem.Execute()
        {
            foreach (var entity in _entities)
            {
                _syncronizator.SyncGlobalPosition(entity.Id, entity.GlobalPosition);
            }
        }
    }
}