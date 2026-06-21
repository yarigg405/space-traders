using Entitas;
using System.Collections.Generic;


namespace Assets.Code.ServerPart.Gameplay.Features.Movement.Systems
{
    internal sealed class ShipFinishUndockSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _entities;
        private readonly List<GameEntity> _buffer = new(4);

        public ShipFinishUndockSystem(GameContext game)
        {
            _entities = game.GetGroup(
                GameMatcher.AllOf(
                    GameMatcher.AnimatedMovingDataContainer)
                .NoneOf(
                    GameMatcher.DockingInProccess,
                    GameMatcher.UndockingInProccess));
        }

        void IExecuteSystem.Execute()
        {
            foreach (var entity in _entities.GetEntities(_buffer))
            {
                entity.RemoveAnimatedMovingDataContainer();
            }
        }
    }
}
