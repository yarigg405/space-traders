using Assets.Code.Common.Physics.Services;
using Entitas;
using System.Collections.Generic;


namespace Assets.Code.ServerPart.Gameplay.Features.Physics.Systems
{
    internal sealed class InitializeStationsSystem : IExecuteSystem
    {
        private readonly IPhysicsRegistrar _physicsRegistrar;
        private readonly IGroup<GameEntity> _entities;
        private readonly List<GameEntity> _buffer = new(32);

        public InitializeStationsSystem(IPhysicsRegistrar physicsRegistrar, GameContext game)
        {
            _physicsRegistrar = physicsRegistrar;
            _entities = game.GetGroup(GameMatcher.AllOf(
                GameMatcher.Station,
                GameMatcher.QuadrantIndex,
                GameMatcher.NeedInit
                ));
        }

        void IExecuteSystem.Execute()
        {
            foreach (var entity in _entities.GetEntities(_buffer))
            {
                _physicsRegistrar.RefreshPositionFor(entity);
                entity.isNeedInit = false;
            }
        }
    }
}
