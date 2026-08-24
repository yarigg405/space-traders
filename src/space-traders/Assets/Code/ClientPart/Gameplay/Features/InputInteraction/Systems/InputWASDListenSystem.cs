using Assets.Code.ClientPart.Networking;
using Entitas;
using UnityEngine;


namespace Assets.Code.ClientPart.Gameplay.Features.InputInteraction.Systems
{
    internal sealed class InputWASDListenSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _players;

        private readonly InputReferencesContainer _input;
        private readonly ClientMessenger _messenger;

        private const float _rotationModifier = 150f;
        private const float _speedModifier = 1f;

        public InputWASDListenSystem(ClientMessenger messenger,
            InputReferencesContainer input, GameContext game)
        {
            _messenger = messenger;
            _input = input;

            _players = game.GetGroup(GameMatcher.ClientPlayer);
        }

        void IExecuteSystem.Execute()
        {
            foreach (var player in _players)
            {
                var moveValue = _input.Actions.Player.Move.ReadValue<Vector2>();
                _messenger.SendMoveInput(moveValue);
            }
        }
    }
}
