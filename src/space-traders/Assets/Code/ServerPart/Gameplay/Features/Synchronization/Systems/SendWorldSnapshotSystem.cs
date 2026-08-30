using Assets.Code.Common.Time;
using Assets.Code.ServerPart.Networking;
using Entitas;
using System.Collections.Generic;


namespace Assets.Code.ServerPart.Gameplay.Features.Synchronization.Systems
{
    internal sealed class SendWorldSnapshotSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _dynamicObjects;
        private readonly IGroup<GameEntity> _players;
        private readonly ServerMessenger _messenger;
        private readonly TickCounter _tick;
        private readonly List<GameEntity> _buffer = new(256);


        public SendWorldSnapshotSystem(GameContext game, ServerMessenger messenger, TickCounter tick)
        {
            _dynamicObjects = game.GetGroup(GameMatcher.AllOf(
                GameMatcher.NetworkTransform,
                GameMatcher.GlobalPosition,
                GameMatcher.CurrentRotationY
                ));

            _players = game.GetGroup(GameMatcher.AllOf(
                GameMatcher.Player,
                GameMatcher.PlayerNetworkId
                ));

            _messenger = messenger;
            _tick = tick;
        }

        void IExecuteSystem.Execute()
        {
            foreach (var receiver in _players)
                _messenger.SendWorldSnapshot(receiver.PlayerNetworkId,
                    _tick.CurrentTick, _dynamicObjects.GetEntities(_buffer));
        }
    }
}
