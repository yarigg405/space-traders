using Entitas;


namespace Assets.Code.ClientPart.Gameplay.Features.InputInteraction.Systems
{
    internal sealed class CapturePreviousTickStateSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _entities;

        public CapturePreviousTickStateSystem(GameContext game)
        {
            _entities = game.GetGroup(GameMatcher.AllOf(
                GameMatcher.ClientPlayer,
                GameMatcher.GlobalPosition
                ));
        }

        void IExecuteSystem.Execute()
        {
            foreach (var entity in _entities)
            {
                entity.ReplacePreviousTickGlobalPosition(entity.GlobalPosition);

                if (entity.hasCurrentRotationY)
                    entity.ReplacePreviousTickRotationY(entity.CurrentRotationY);
            }
        }
    }
}