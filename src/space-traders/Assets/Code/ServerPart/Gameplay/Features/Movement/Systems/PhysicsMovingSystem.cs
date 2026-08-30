using Assets.Code.Common.Extensions;
using Entitas;


namespace Assets.Code.ServerPart.Gameplay.Features.Movement.Systems
{
    internal sealed class PhysicsMovingSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _movers;

        public PhysicsMovingSystem(GameContext game)
        {
            _movers = game.GetGroup(GameMatcher.AllOf(
                GameMatcher.Moving,
                GameMatcher.Velocity,
                GameMatcher.GlobalPosition
            ));
        }

        void IExecuteSystem.Execute()
        {
            foreach (var mover in _movers)
            {
                MovementFormulas.Move(mover); 
            }
        }
    }
}
