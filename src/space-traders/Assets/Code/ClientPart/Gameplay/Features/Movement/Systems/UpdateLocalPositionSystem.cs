using Assets.Code.Common;
using Assets.Code.Common.Time;
using Entitas;
using Unity.Mathematics;
using UnityEngine;


namespace Assets.Code.ClientPart.Gameplay.Features.Movement.Systems
{
    internal sealed class UpdateLocalPositionSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _players;
        private readonly IGroup<GameEntity> _entities;
        private readonly InterpolationClock _interpolation;

        public UpdateLocalPositionSystem(GameContext game, InterpolationClock interpolation)
        {
            _players = game.GetGroup(GameMatcher.ClientPlayer);

            _entities = game.GetGroup(GameMatcher.AllOf(
                GameMatcher.GlobalPosition,
                GameMatcher.QuadrantIndex
            ));
            _interpolation = interpolation;
        }

        void IExecuteSystem.Execute()
        {
            var quadrantSize = GameConstants.GAME_SCENE_QUADRANT_SIZE;

            foreach (var player in _players)
            {
                var playerQuadrant = player.QuadrantIndex;

                var a = _interpolation.Alpha;
                foreach (var entity in _entities)
                {
                    var objectGlobalPos = entity.GlobalPosition;
                    var objectQuadrant = entity.QuadrantIndex;

                    var renderGlobal = entity.hasPreviousTickGlobalPosition ?
                        math.lerp(entity.PreviousTickGlobalPosition, entity.GlobalPosition, (double)a) : 
                        entity.GlobalPosition;

                    double objectLocalX = renderGlobal.x - entity.QuadrantIndex.x * quadrantSize;
                    double objectLocalY = renderGlobal.y - entity.QuadrantIndex.y * quadrantSize;

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