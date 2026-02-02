using Assets.Code.Common.Physics.Services;
using Entitas;


namespace Assets.Code.ServerPart.Gameplay.Features.Physics.Systems
{
    internal sealed class UpdatePhysicsQuadrantsSystem : IExecuteSystem
    {
        private readonly IPhysicsRegistrar _physicsRegistrar;
        private readonly IGroup<GameEntity> _entities;

        public UpdatePhysicsQuadrantsSystem(IPhysicsRegistrar physicsRegistrar, GameContext game)
        {
            _physicsRegistrar = physicsRegistrar;
            _entities = game.GetGroup(GameMatcher.AllOf(
                GameMatcher.PhysicRadius,
                GameMatcher.QuadrantIndex
                ));
        }

        void IExecuteSystem.Execute()
        {
            foreach (var entity in _entities)
            {
                _physicsRegistrar.RefreshPositionFor(entity);
            }
        }
    }
}
