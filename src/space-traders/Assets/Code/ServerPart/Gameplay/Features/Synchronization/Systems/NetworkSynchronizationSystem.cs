using Assets.Code.ServerPart.Worlds.GameSynchronization;
using Entitas;


namespace Assets.Code.ServerPart.Gameplay.Features.Synchronization.Systems
{
    internal sealed class NetworkSynchronizationSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _entities;
        private readonly EntitiesSynchronizator _synchronizator;

        private int _currentFrame;

        public NetworkSynchronizationSystem(GameContext game, EntitiesSynchronizator syncronizator)
        {
            _entities = game.GetGroup(
                GameMatcher.AllOf(
                    GameMatcher.GlobalPosition, 
                    GameMatcher.NeedSynchronize)
                .NoneOf(GameMatcher.Warping));

            _synchronizator = syncronizator;
        }

        void IExecuteSystem.Execute()
        {
            _currentFrame++;
            if (_currentFrame < 60) return;

            _currentFrame = 0;
            foreach (var entity in _entities)
            {
                _synchronizator.SyncGlobalPosition(entity.Id, entity.GlobalPosition);
                if (entity.hasCurrentRotationY)
                    _synchronizator.SyncRotation(entity.Id, entity.CurrentRotationY);
            }
        }
    }
}