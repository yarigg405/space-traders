using Assets.Code.Common.Extensions;
using Entitas;


namespace Assets.Code.ServerPart.Gameplay.Features.Movement.Systems
{
    internal sealed class UpdateMovingSpeedSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _entities;

        public UpdateMovingSpeedSystem(GameContext game)
        {
            _entities = game.GetGroup(GameMatcher.AllOf(
                GameMatcher.MaxMoveSpeed,
                GameMatcher.CurrentSpeedModifier,
                GameMatcher.CurrentMoveSpeed
                ));
        }

        void IExecuteSystem.Execute()
        {
            foreach (var entity in _entities)
            {
                MovementFormulas.UpdateMoveSpeed(entity);
            }
        }
    }
}
