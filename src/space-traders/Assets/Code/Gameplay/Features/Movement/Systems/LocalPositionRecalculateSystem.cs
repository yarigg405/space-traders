using Assets.Code.Common;
using Entitas;
using System;
using UnityEngine;


namespace Assets.Code.Gameplay.Features.Movement.Systems
{
    internal sealed class LocalPositionRecalculateSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _entities;

        internal LocalPositionRecalculateSystem(GameContext game)
        {
            _entities = game.GetGroup(GameMatcher.AllOf(
                GameMatcher.GlobalPosition
            ));
        }

        void IExecuteSystem.Execute()
        {
            foreach (var entity in _entities)
            {
                var quadrantX = (int)Math.Floor(entity.GlobalPosition.X / GameConstants.GameSceneAbsoluteSize);
                var quadrantY = (int)Math.Floor(entity.GlobalPosition.Y / GameConstants.GameSceneAbsoluteSize);
                entity.ReplaceQuadrantIndex(new Vector2Int(quadrantX, quadrantY));

                var localX = (float)(entity.GlobalPosition.X % GameConstants.GameSceneAbsoluteSize);
                var localY = (float)(entity.GlobalPosition.Y % GameConstants.GameSceneAbsoluteSize);
                entity.ReplaceLocalPosition(new Vector3(localX,0, localY));
            }
        }
    }
}