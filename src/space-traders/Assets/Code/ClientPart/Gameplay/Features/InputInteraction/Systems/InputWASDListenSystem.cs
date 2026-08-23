using Assets.Code.ClientPart.Networking;
using Assets.Code.Common.Time;
using Entitas;
using Unity.Mathematics;
using UnityEngine;


namespace Assets.Code.ClientPart.Gameplay.Features.InputInteraction.Systems
{
    internal sealed class InputWASDListenSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _players;

        private readonly InputReferencesContainer _input;
        private readonly ITimeService _time;
        private readonly ClientMessenger _messenger;

        private const float _rotationModifier = 150f;
        private const float _speedModifier = 1f;

        public InputWASDListenSystem(ClientMessenger messenger,
            InputReferencesContainer input, ITimeService time, GameContext game)
        {
            _messenger = messenger;
            _input = input;
            _time = time;

            _players = game.GetGroup(GameMatcher.ClientPlayer);
        }

        void IExecuteSystem.Execute()
        {
            foreach (var player in _players)
            {
                var moveValue = _input.Actions.Player.Move.ReadValue<Vector2>();

                if (moveValue.sqrMagnitude < 0.001f)
                    continue;

                var dt = _time.DeltaTime;

                var currentRotation = player.TargetRotation;
                currentRotation += moveValue.x * _rotationModifier * dt;
                _messenger.SendTargetRotationToServer(currentRotation);

                var currentSpeed = player.CurrentSpeedModifier;
                currentSpeed += moveValue.y * _speedModifier * dt;
                currentSpeed = math.clamp(currentSpeed, 0f, 1f);

                _messenger.SendSpeedModifierToServer(currentSpeed);
            }
        }
    }
}
