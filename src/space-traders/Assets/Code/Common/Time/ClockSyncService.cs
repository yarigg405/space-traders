using Assets.Code.ClientPart.Networking;
using Assets.Code.Networking;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using VContainer.Unity;


namespace Assets.Code.Common.Time
{
    public sealed class ClockSyncService : ITickable
    {
        private const float _pingInterval = 0.5f;
        private const int _jitterBufferTicks = 2;
        private const int _hardResnapTicks = 5;

        private readonly ClientMessenger _messenger;
        private readonly TickCounter _clientTick;
        private readonly NetworkManager _network;

        private readonly Dictionary<uint, float> _pingSentAt = new();
        private uint _pingId;
        private float _sincePing;
        private bool _synced;

        public float TimeScale { get; private set; } = 1f;

        public ClockSyncService(ClientMessenger messenger,
            TickCounter clientTick, NetworkManager network)
        {
            _messenger = messenger;
            _clientTick = clientTick;
            _network = network;
        }

        void ITickable.Tick()
        {
            if (!_network.Client.IsConnected)
            {
                _synced = false;
                TimeScale = 1f;
                return;
            }

            _sincePing += UnityEngine.Time.unscaledDeltaTime;
            if (_sincePing < _pingInterval) return;

            _sincePing = 0f;
            _pingSentAt[++_pingId] = UnityEngine.Time.realtimeSinceStartup;
            _messenger.SendPing(_pingId);
        }

        public void OnPong(uint pingId, uint serverTick)
        {
            if (!_pingSentAt.TryGetValue(pingId, out var sentAt)) return;
            _pingSentAt.Remove(pingId);

            var rtt = UnityEngine.Time.realtimeSinceStartup - sentAt;
            var rttTicks = Mathf.RoundToInt(rtt / GameConstants.FIXED_DELTA_TIME);
            var target = (uint)((int)serverTick + rttTicks + _jitterBufferTicks);

            var error = (int)target - (int)_clientTick.CurrentTick;

            if (!_synced || Mathf.Abs(error) > _hardResnapTicks)
            {
                _clientTick.SetupTick(target);
                TimeScale = 1f;
                _synced = true;
            }
            else
            {
                TimeScale = 1f + Mathf.Clamp(error, -3, 3) * 0.02f;
            }
        }
    }
}
