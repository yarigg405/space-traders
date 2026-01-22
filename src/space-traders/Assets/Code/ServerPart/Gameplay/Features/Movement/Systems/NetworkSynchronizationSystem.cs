using Assets.Code.ServerPart.Worlds.GameSynchronization;
using Entitas;


namespace Assets.Code.ServerPart.Gameplay.Features.Movement.Systems
{
    internal sealed class NetworkSynchronizationSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _entities;
        private readonly EntitiesSynchronizator _synchronizator;

        public NetworkSynchronizationSystem(GameContext game, EntitiesSynchronizator syncronizator)
        {
            _entities = game.GetGroup(GameMatcher.GlobalPosition);
            _synchronizator = syncronizator;
        }

        void IExecuteSystem.Execute()
        {
            foreach (var entity in _entities)
            {
                _synchronizator.SyncGlobalPosition(entity.Id, entity.GlobalPosition);
                _synchronizator.SyncRotation(entity.Id, entity.CurrentRotationY);
            }
        }
    }
}