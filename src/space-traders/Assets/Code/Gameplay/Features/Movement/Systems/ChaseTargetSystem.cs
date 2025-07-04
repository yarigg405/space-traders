using Assets.Code.Gameplay.Common;
using Entitas;


namespace Assets.Code.Gameplay.Features.Movement.Systems
{
    internal sealed class ChaseTargetSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _chasers;

        internal ChaseTargetSystem(GameContext game)
        {
            _chasers = game.GetGroup(GameMatcher.AllOf(
                GameMatcher.ChaseTarget,
                GameMatcher.TargetRotation,
                GameMatcher.GlobalPosition
            ));
        }

        void IExecuteSystem.Execute()
        {
            foreach (var chaser in _chasers)
            {
                if (!chaser.ChaseTarget.hasGlobalPosition) continue;
                var direction = CommonExtensions.GetDirectionAngleFromTo(chaser.GlobalPosition, chaser.ChaseTarget.GlobalPosition);

                chaser.ReplaceTargetRotation(direction);

                chaser.isMoving = true;
            }
        }
    }
}