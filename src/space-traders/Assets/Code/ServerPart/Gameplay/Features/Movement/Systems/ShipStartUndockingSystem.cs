using Assets.Code.Common.DataContainers;
using Entitas;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Code.ServerPart.Gameplay.Features.Movement.Systems
{
    internal sealed class ShipStartUndockingSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _entities;
        private readonly List<GameEntity> _buffer = new(4);

        private const float _dockingTime = 5f;

        public ShipStartUndockingSystem(GameContext game)
        {
            _entities = game.GetGroup(GameMatcher.AllOf(
               GameMatcher.UndockingInProcess
               )
                .NoneOf(GameMatcher.AnimatedMovingDataContainer));
        }

        void IExecuteSystem.Execute()
        {
            foreach (var entity in _entities.GetEntities(_buffer))
            {
                var to = Vector3.zero;
                var from = new Vector3(0f, -25f, 0f);

                var container = new AnimatedMoveDataContainer(from, to, _dockingTime);

                entity.AddAnimatedMovingDataContainer(container);
            }
        }
    }
}
