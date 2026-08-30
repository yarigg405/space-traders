using Assets.Code.ClientPart.Gameplay.Features.InputInteraction;
using Assets.Code.Common;
using Unity.Mathematics;
using UnityEngine;


namespace Assets.Code.ClientPart.Gameplay.Features.Player.Infrastructure
{
    public sealed class PlayerReconciler
    {
        private readonly GameContext _game;
        private readonly ClientCommandBuffer _buffer;
        private uint _lastTick;

        public PlayerReconciler(GameContext game, ClientCommandBuffer buffer)
        {
            _game = game;
            _buffer = buffer;
        }

        public void OnSnapshot(uint entityId, uint serverTick, double2 pos, float rot,
            Vector2 vel, float moveSpeed, float targetRotation, float speedModifier, bool isWarping)
        {
            if (serverTick <= _lastTick) return;
            _lastTick = serverTick;

            var player = _game.GetEntityWithId(entityId);
            if (player == null) return;

            _buffer.AckUpTo(serverTick);
            player.isWarping = isWarping;

            player.ReplaceGlobalPosition(pos);
            player.ReplaceCurrentRotationY(rot);
            player.ReplaceVelocity(vel);
            player.ReplaceCurrentMoveSpeed(moveSpeed);
            player.ReplaceTargetRotation(targetRotation);
            player.ReplaceCurrentSpeedModifier(speedModifier);

            if (!isWarping)
                foreach (var cmd in _buffer.Unacked)
                {
                    player.ReplaceMoveInput(cmd.MoveInput);
                    PlayerSimulationStep.Apply(player);
                }

            player.ReplaceQuadrantIndex(player.GlobalPosition.ToQuadrantIndex());

            player.ReplacePreviousTickGlobalPosition(player.GlobalPosition);
            player.ReplacePreviousTickRotationY(player.CurrentRotationY);
        }
    }
}
