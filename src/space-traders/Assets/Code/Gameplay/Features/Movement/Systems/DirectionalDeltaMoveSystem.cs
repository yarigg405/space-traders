using Assets.Code.Gameplay.Common.Time;
using Entitas;


namespace Assets.Code.Gameplay.Features.Movement.Systems
{
    internal sealed class DirectionalDeltaMoveSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _movers;
        private readonly ITimeService _time;

        internal DirectionalDeltaMoveSystem(GameContext game, ITimeService time)
        {
            _time = time;

            _movers = game.GetGroup(GameMatcher.AllOf(
                GameMatcher.Moving,
                GameMatcher.DirectionalMovement,
                GameMatcher.GlobalPosition
            ));
        }

        void IExecuteSystem.Execute()
        {
            foreach (var mover in _movers)
            {
                var pos = mover.GlobalPosition;
                pos.X += mover.DirectionalMovement.x * _time.DeltaTime;
                pos.Y += mover.DirectionalMovement.y * _time.DeltaTime;

                mover.ReplaceGlobalPosition(pos);
            }
        }
    }
}