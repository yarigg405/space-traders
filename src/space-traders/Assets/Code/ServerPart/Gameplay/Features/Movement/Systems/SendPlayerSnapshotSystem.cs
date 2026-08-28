using Assets.Code.Common.Time;
using Assets.Code.ServerPart.Networking;
using Entitas;


namespace Assets.Code.ServerPart.Gameplay.Features.Movement.Systems
{
    internal sealed class SendPlayerSnapshotSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _players;
        private readonly ServerMessenger _messenger;
        private readonly TickCounter _tick;

        public SendPlayerSnapshotSystem(GameContext game, ServerMessenger messenger, TickCounter tick)
        {
            _players = game.GetGroup(GameMatcher.AllOf(
                GameMatcher.Player,
                GameMatcher.PlayerNetworkId,
                GameMatcher.GlobalPosition,
                GameMatcher.CurrentRotationY,
                GameMatcher.Velocity,
                GameMatcher.CurrentMoveSpeed,
                GameMatcher.TargetRotation,
                GameMatcher.CurrentSpeedModifier
                ));


            _messenger = messenger;
            _tick = tick;
        }

        void IExecuteSystem.Execute()
        {
            foreach (var player in _players)
            {
                _messenger.SendPlayerSnapshot(player.PlayerNetworkId, player.Id, _tick.CurrentTick,
                    player.GlobalPosition, player.CurrentRotationY, player.Velocity, player.CurrentMoveSpeed,
                    player.TargetRotation, player.CurrentSpeedModifier);
            }
        }
    }
}
