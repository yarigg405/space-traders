using Entitas;


namespace Assets.Code.Gameplay.Features.Movement.Systems
{
    internal sealed class UpdateTransformPositionSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _entities;

        internal UpdateTransformPositionSystem(GameContext game)
        {
            _entities = game.GetGroup(GameMatcher.AllOf(
                GameMatcher.Transform,
                GameMatcher.LocalPosition
            ));
        }

        void IExecuteSystem.Execute()
        {
            foreach (var entity in _entities)
            {
                entity.Transform.position = entity.LocalPosition;
            }
        }
    }
}