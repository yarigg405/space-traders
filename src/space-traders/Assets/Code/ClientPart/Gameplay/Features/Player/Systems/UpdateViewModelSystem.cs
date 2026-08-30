using Entitas;


namespace Assets.Code.ClientPart.Gameplay.Features.Player.Systems
{
    internal sealed class UpdateViewModelSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _entities;

        public UpdateViewModelSystem(GameContext game)
        {
            _entities = game.GetGroup(GameMatcher.AllOf(
                GameMatcher.ViewModel,
                GameMatcher.QuadrantIndex
                ));
        }

        void IExecuteSystem.Execute()
        {
            foreach (var entity in _entities)
            {
                entity.ViewModel.UpdateModel(entity);
            }
        }
    }
}
