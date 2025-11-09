using Entitas;
using UnityEngine;


namespace Assets.Code.ClientPart.Gameplay.Features.Movement.Systems
{
    internal sealed class UpdateLocalPositionSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _entities;

        internal UpdateLocalPositionSystem(GameContext game)
        {
            _entities = game.GetGroup(GameMatcher.AllOf(
                GameMatcher.GlobalPosition
            ));
        }

        void IExecuteSystem.Execute()
        {
            foreach (var entity in _entities)
            {
                var globalPos = entity.GlobalPosition;
                var localPos = new Vector3((float)globalPos.x, 0, (float)globalPos.y);

                entity.Transform.position = localPos;
            }
        }
    }
}