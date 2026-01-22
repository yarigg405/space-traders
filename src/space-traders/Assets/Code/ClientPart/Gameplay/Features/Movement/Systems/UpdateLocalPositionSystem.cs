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
            _players = game.GetGroup(GameMatcher.ClientPlayer);

            _entities = game.GetGroup(GameMatcher.AllOf(
                GameMatcher.GlobalPosition,
                GameMatcher.QuadrantIndex
            ));
        }

        void IExecuteSystem.Execute()
        {
            var quadrantSize = GameConstants.GAME_SCENE_QUADRANT_SIZE;

            foreach (var player in _players)
            {
                var playerQuadrant = player.QuadrantIndex;

                foreach (var entity in _entities)
                {
                    var objectGlobalPos = entity.GlobalPosition;
                    var objectQuadrant = entity.QuadrantIndex;

                    double objectLocalX = objectGlobalPos.x - objectQuadrant.x * quadrantSize;
                    double objectLocalY = objectGlobalPos.y - objectQuadrant.y * quadrantSize;

                    int deltaX = objectQuadrant.x - playerQuadrant.x;
                    int deltaY = objectQuadrant.y - playerQuadrant.y;

                    double relativeX = (objectLocalX) + deltaX * quadrantSize;
                    double relativeY = (objectLocalY) + deltaY * quadrantSize;

                    var newLocal = new Vector3((float)relativeX, 0f, (float)relativeY);

                    entity.ReplaceLocalPosition(newLocal);
                }
            }
        }
    }
}