using Entitas;


namespace Assets.Code.Gameplay.Features.DamageApplication.Systems
{
    public sealed class DestructOnZeroHpSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _entities;

        internal DestructOnZeroHpSystem(GameContext game)
        {
            _entities = game.GetGroup(GameMatcher.AllOf(
                GameMatcher.CurrentHp
            ));
        }

        void IExecuteSystem.Execute()
        {
            foreach (var entity in _entities)
            {
                if (entity.CurrentHp <= 0)
                    entity.isDestructed = true;
            }
        }
    }
}