using Assets.Code.ClientPart.Networking;
using Assets.Code.Networking;
using System.Collections.Generic;
using UnityEngine;
using VContainer.Unity;


namespace Assets.Code.Common.Time
{
    public sealed class ClockSyncService : ITickable
    {
        private const float _pingInterval = 0.5f;
        private const int _jitterBufferTicks = 5;
        private const int _hardResnapTicks = 8;

        private readonly ClientMessenger _messenger;
        private readonly TickCounter _clientTick;
        private readonly NetworkManager _network;

        private readonly Dictionary<uint, float> _pingSentAt = new();
        private uint _pingId;
        private uint _lastPongId;
        private float _sincePing;
        private float _smoothedRtt;

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
            if (pingId <= _lastPongId) return;

            if (!_pingSentAt.TryGetValue(pingId, out var sentAt)) return;
            _pingSentAt.Remove(pingId);

            var rtt = UnityEngine.Time.realtimeSinceStartup - sentAt;

            _smoothedRtt = _smoothedRtt < 0f ? rtt : Mathf.Lerp(_smoothedRtt, rtt, 0.2f);

            var rttTicks = Mathf.RoundToInt(_smoothedRtt / GameConstants.FIXED_DELTA_TIME);
            var target = (uint)((int)serverTick + rttTicks + _jitterBufferTicks);
            var error = (int)target - (int)_clientTick.CurrentTick;

            if (Mathf.Abs(error) > _hardResnapTicks)
            {
                _clientTick.SetupTick(target);
                TimeScale = 1f;
            }
            else
            {
                TimeScale = 1f + Mathf.Clamp(error, -2, 2) * 0.05f;
            }
        }
    }
}