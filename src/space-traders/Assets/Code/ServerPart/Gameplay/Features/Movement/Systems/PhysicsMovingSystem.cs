using Assets.Code.Common.Time;
using Entitas;


namespace Assets.Code.ServerPart.Gameplay.Features.Movement.Systems
{
    internal sealed class PhysicsMovingSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _movers;
        private readonly ITimeService _time;

        public PhysicsMovingSystem(GameContext game, ITimeService time)
        {
            _time = time;

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

                pos.x += mover.Velocity.x * _time.DeltaTime;
                pos.y += mover.Velocity.y * _time.DeltaTime;

                mover.ReplaceGlobalPosition(pos);
                mover.isNeedSynchronize = true;
            }
        }
    }
}
