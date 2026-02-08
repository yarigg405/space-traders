using Assets.Code.Common;
using Entitas;


namespace Assets.Code.ServerPart.Gameplay.Features.Movement.Systems
{
    internal sealed class UpdateQuadrantIndexSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _entities;

        public UpdateQuadrantIndexSystem(GameContext game)
        {
            _entities = game.GetGroup(GameMatcher.AllOf(
                GameMatcher.GlobalPosition
            ));
        }

        void IExecuteSystem.Execute()
        {
            foreach (var entity in _entities)
            {
                var quadrantIndex = entity.GlobalPosition.ToQuadrantIndex();
                entity.ReplaceQuadrantIndex(quadrantIndex);
            }
        }
    }
}
