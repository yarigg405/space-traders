using Assets.Code.Common;
using Entitas;
using System;


namespace Assets.Code.ServerPart.Gameplay.Features.Movement.Systems
{
    internal sealed class UpdateQuadrantIndexSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _entities;

        public UpdateQuadrantIndexSystem(GameContext game)
        {
            _entities = game.GetGroup(GameMatcher.AllOf(
                GameMatcher.GlobalPosition
            ));
        }

        void IExecuteSystem.Execute()
        {
            foreach (var entity in _entities)
            {
                var quadrantX = (int)Math.Floor(entity.GlobalPosition.x / GameConstants.GAME_SCENE_QUADRANT_SIZE);
                var quadrantY = (int)Math.Floor(entity.GlobalPosition.y / GameConstants.GAME_SCENE_QUADRANT_SIZE);
                entity.ReplaceQuadrantIndex(new(quadrantX, quadrantY));
            }
        }
    }
}
