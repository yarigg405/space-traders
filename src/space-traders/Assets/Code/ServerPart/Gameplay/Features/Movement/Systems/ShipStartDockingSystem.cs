using Assets.Code.Common.DataContainers;
using Assets.Code.Common.Time;
using Entitas;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Code.ServerPart.Gameplay.Features.Movement.Systems
{
    internal sealed class ShipStartDockingSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _entities;
        private readonly List<GameEntity> _buffer = new(4);

        private readonly ITimeService _time;
        private readonly GameContext _context;

        private float _dockingTime = 5f;

        public ShipStartDockingSystem(GameContext game, ITimeService time)
        {
            _time = time;
            _context = game;

            _entities = game.GetGroup(GameMatcher.AllOf(
               GameMatcher.ShipCanBeDocked,
               GameMatcher.DockingInProccess
               )
                .NoneOf(GameMatcher.AnimatedMovingDataContainer));
        }

        void IExecuteSystem.Execute()
        {
            foreach (var entity in _entities.GetEntities(_buffer))
            {
                var initData = entity.ShipCanBeDocked;
                var from = entity.Transform.position;
                var to = new Vector3(from.x, -25f, from.z);

                var container = new AnimatedMoveDataContainer(from, to, _dockingTime);

                entity.AddAnimatedMovingDataContainer(container);
            }
        }
    }
}
