using Entitas;
using System;
using System.Collections.Generic;


namespace Assets.Code.ClientPart.Gameplay.Features.Navigation
{
    public sealed class NavigationRegistry : IDisposable
    {
        private readonly IGroup<GameEntity> _group;
        private readonly HashSet<GameEntity> _objects = new();

        public IReadOnlyCollection<GameEntity> Objects => _objects;
        public event Action<GameEntity> Added;
        public event Action<GameEntity> Removed;

        public NavigationRegistry(GameContext game)
        {
            _group = game.GetGroup(GameMatcher.AnyOf(GameMatcher.Station, GameMatcher.Planet));

            foreach (var entity in _group.GetEntities())
                _objects.Add(entity);

            _group.OnEntityAdded += OnEntityAddedToGroup;
            _group.OnEntityRemoved += OnEntityRemovedFromGroup;
        }

        void IDisposable.Dispose()
        {
            _group.OnEntityAdded -= OnEntityAddedToGroup;
            _group.OnEntityRemoved -= OnEntityRemovedFromGroup;
            _objects.Clear();
            Added = null;
            Removed = null;
        }

        private void OnEntityAddedToGroup(IGroup<GameEntity> group, GameEntity entity, int index, IComponent component)
        {
            if (_objects.Add(entity))
                Added?.Invoke(entity);
        }

        private void OnEntityRemovedFromGroup(IGroup<GameEntity> group, GameEntity entity, int index, IComponent component)
        {
            if (_objects.Remove(entity))
                Removed?.Invoke(entity);
        }
    }
}
