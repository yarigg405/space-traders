using Assets.Code.Common;
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
                var pos = mover.GlobalPosition;

                pos.x += mover.Velocity.x * GameConstants.FIXED_DELTA_TIME;
                pos.y += mover.Velocity.y * GameConstants.FIXED_DELTA_TIME;

                mover.ReplaceGlobalPosition(pos);
                mover.isNeedSynchronize = true;
            }
        }
    }
}
