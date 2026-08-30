using Assets.Code.Common.Extensions;
using Entitas;


namespace Assets.Code.ServerPart.Gameplay.Features.Movement.Systems
{
    internal sealed class HandleVelocitySystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _entities;

        public HandleVelocitySystem(GameContext game)
        {
            _entities = game.GetGroup(GameMatcher.AllOf(
                GameMatcher.CurrentRotationY,
                GameMatcher.CurrentMoveSpeed,
                GameMatcher.VelocityAgility
            ));
        }

        void IExecuteSystem.Execute()
        {
            foreach (var entity in _entities)
            {
                MovementFormulas.HandleVelocity(entity);
            }
        }
    }
}
