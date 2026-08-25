using Assets.Code.ClientPart.Networking;
using Assets.Code.Common.Time;
using Entitas;
using UnityEngine;


namespace Assets.Code.ClientPart.Gameplay.Features.InputInteraction.Systems
{
    internal sealed class PlayerCommandSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _players;
        private readonly InputReferencesContainer _input;
        private readonly ClientMessenger _messenger;
        private readonly ClientCommandBuffer _buffer;
        private readonly TickCounter _tickCounter;


        public PlayerCommandSystem(GameContext game, InputReferencesContainer input,
            ClientMessenger messenger, ClientCommandBuffer buffer, TickCounter tickCounter)
        {
            _players = game.GetGroup(GameMatcher.ClientPlayer);

            _input = input;
            _messenger = messenger;
            _buffer = buffer;
            _tickCounter = tickCounter;
        }

        void IExecuteSystem.Execute()
        {
            foreach (var player in _players)
            {
                var moveInput = _input.Actions.Player.Move.ReadValue<Vector2>();
                var cmd = new InputCommand(_tickCounter.CurrentTick, moveInput);

                _buffer.Add(cmd);
                _messenger.SendMoveInput(cmd.Tick, moveInput);
                player.ReplaceMoveInput(moveInput);
            }
        }
    }
}
