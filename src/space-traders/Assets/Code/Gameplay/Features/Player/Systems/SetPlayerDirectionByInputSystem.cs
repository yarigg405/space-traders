using Entitas;
using Yrr.Utils;


namespace Assets.Code.Gameplay.Features.Player.Systems
{
    internal sealed class SetPlayerDirectionByInputSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _players;
        private readonly IGroup<GameEntity> _inputs;


        internal SetPlayerDirectionByInputSystem(GameContext game)
        {
            _players = game.GetGroup(GameMatcher.AllOf(
                GameMatcher.Player,
                GameMatcher.TargetRotation,
                GameMatcher.LocalPosition
            ));
            _inputs = game.GetGroup(GameMatcher.AllOf(
                GameMatcher.Input,
                GameMatcher.ClickedPosition)
                );
        }

        void IExecuteSystem.Execute()
        {
            foreach (var input in _inputs)
            {
                foreach (var player in _players)
                {
                    var targetRotation = AnglesUtil.GetAngleDirectionY(player.LocalPosition, input.ClickedPosition);
                    player.ReplaceTargetRotation(targetRotation);
                    player.ReplaceMoveSpeed(15f);
                    player.isMoving = true;
                }
            }
        }
    }
}