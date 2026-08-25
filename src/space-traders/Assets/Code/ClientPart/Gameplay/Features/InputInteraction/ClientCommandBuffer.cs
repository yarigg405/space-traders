using System.Collections.Generic;
using UnityEngine;


namespace Assets.Code.ClientPart.Gameplay.Features.InputInteraction
{
    public sealed class ClientCommandBuffer
    {
        private readonly List<InputCommand> _commands = new();

        public void Add(InputCommand cmd) =>_commands.Add(cmd);
        public void AckUpTo(uint tick) => _commands.RemoveAll(c=> c.Tick <= tick);
        public IReadOnlyList<InputCommand> Unacked => _commands;

    }

    public readonly struct InputCommand
    {
        public readonly uint Tick;
        public readonly Vector2 MoveInput;

        public InputCommand(uint tick, Vector2 moveInput)
        {
            Tick = tick; MoveInput = moveInput;
        }
    }
}
