using Entitas;
using UnityEngine;


namespace Assets.Code.Common.Destruct.Systems
{
    internal sealed class CleanupGameDestructedViewSystem : ICleanupSystem
    {
        private readonly IGroup<GameEntity> _entities;

        internal CleanupGameDestructedViewSystem(GameContext game)
        {
            _entities = game.GetGroup(GameMatcher.AllOf(
                GameMatcher.View,
                GameMatcher.Destructed
            ));
        }

        void ICleanupSystem.Cleanup()
        {
            foreach (var entity in _entities)
            {
                entity.View.ReleaseEntity();
                Object.Destroy(entity.View.gameObject);
            }
        }
    }
}