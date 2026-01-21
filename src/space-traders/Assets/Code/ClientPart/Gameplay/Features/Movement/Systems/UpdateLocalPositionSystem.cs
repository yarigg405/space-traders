using Assets.Code.Common;
using Entitas;
using UnityEngine;


namespace Assets.Code.ClientPart.Gameplay.Features.Movement.Systems
{
    internal sealed class UpdateLocalPositionSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _players;
        private readonly IGroup<GameEntity> _entities;

        public UpdateLocalPositionSystem(GameContext game)
        {
            _players = game.GetGroup(GameMatcher.Player);

            _entities = game.GetGroup(GameMatcher.AllOf(
                GameMatcher.GlobalPosition,
                GameMatcher.QuadrantIndex
            ));
        }

        void IExecuteSystem.Execute()
        {
            foreach (var player in _players)
            {
                foreach (var entity in _entities)
                {
                    var quadrantDelta = (entity.QuadrantIndex - player.QuadrantIndex);
                    var offsetX = quadrantDelta.x * GameConstants.GAME_SCENE_QUADRANT_SIZE;
                    var offsetY = quadrantDelta.y * GameConstants.GAME_SCENE_QUADRANT_SIZE;

                    var localX = entity.GlobalPosition.x % GameConstants.GAME_SCENE_QUADRANT_SIZE;
                    var localY = entity.GlobalPosition.y % GameConstants.GAME_SCENE_QUADRANT_SIZE;

                    if (localX < 0) localX += GameConstants.GAME_SCENE_QUADRANT_SIZE;
                    if (localY < 0) localY += GameConstants.GAME_SCENE_QUADRANT_SIZE;

                    var newLocal = new Vector3(
                        (float)(localX + offsetX),
                        0f,
                        (float)(localY + offsetY));

                    entity.ReplaceLocalPosition(newLocal);
                }
            }
        }
    }
}