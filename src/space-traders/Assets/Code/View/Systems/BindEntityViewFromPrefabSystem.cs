using Assets.Code.View.Factories;
using Entitas;
using System.Collections.Generic;


namespace Assets.Code.View.Systems
{
    internal sealed class BindEntityViewFromPrefabSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _entities;
        private readonly IEntityViewFactory _viewFactory;
        private readonly List<GameEntity> _buffer = new(32);

        internal BindEntityViewFromPrefabSystem(GameContext game, IEntityViewFactory viewFactory)
        {
            _entities = game.GetGroup(GameMatcher
                .AllOf(GameMatcher.ViewPrefab)
                .NoneOf(GameMatcher.View));

            _viewFactory = viewFactory;
        }

        void IExecuteSystem.Execute()
        {
            foreach (var entity in _entities.GetEntities(_buffer))
            {
                _viewFactory.CreateViewForEntityFromPrefab(entity);
            }
        }
    }
}