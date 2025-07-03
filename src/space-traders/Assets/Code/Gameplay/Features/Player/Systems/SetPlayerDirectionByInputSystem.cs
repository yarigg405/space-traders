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
                GameMatcher.Transform,
                GameMatcher.DirectionalMovement
            ));
            _inputs = game.GetGroup(GameMatcher.Input);
        }

        void IExecuteSystem.Execute()
        {
            foreach (var input in _inputs)
            {
                foreach (var player in _players)
                {
                    player.Transform.position = input.ClickedPosition;

                    //var direction = (input.ClickedPosition - player.Transform.position).ToVector2XZ().normalized;
                    //player.ReplaceDirectionalMovement(direction);
                }
            }
        }
    }
}