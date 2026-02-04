using Assets.Code.Common;
using Entitas;
using Unity.Mathematics;


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
                var x = entity.GlobalPosition.x;
                var y = entity.GlobalPosition.y;

                int quadrantX = (int)math.floor((x + GameConstants.GAME_SCENE_HALF_QUADRANT_SIZE) / GameConstants.GAME_SCENE_QUADRANT_SIZE);
                int quadrantY = (int)math.floor((y + GameConstants.GAME_SCENE_HALF_QUADRANT_SIZE) / GameConstants.GAME_SCENE_QUADRANT_SIZE);

                entity.ReplaceQuadrantIndex(new(quadrantX, quadrantY));
            }
        }
    }
}
