using Assets.Code.Common.Extensions;
using Entitas;


namespace Assets.Code.ServerPart.Gameplay.Features.Movement.Systems
{
    internal sealed class RotationSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _entities;

        public RotationSystem(GameContext game)
        {
            _entities = game.GetGroup(GameMatcher.AllOf(
                GameMatcher.Moving,
                GameMatcher.CurrentRotationY,
                GameMatcher.TargetRotation,
                GameMatcher.RotationSpeed
                ));
        }

        void IExecuteSystem.Execute()
        {
            foreach (var entity in _entities)
            {
                MovementFormulas.Rotate(entity); 
            }
        }
    }
}
