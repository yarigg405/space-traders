using Assets.Code.Networking;
using Assets.Code.ServerPart.Gameplay.Features.Movement;
using Assets.Code.ServerPart.Gameplay.Features.Player.Infrastructure;
using Assets.Code.ServerPart.Networking;
using Cysharp.Threading.Tasks;
using System.Threading;


namespace Assets.Code.ServerPart.Gameplay.Features.Player.Services
{
    public sealed class ServerDockingService
    {
        private readonly PlayerLocationManager _playerLocationManager;
        private readonly ClientSceneConnector _clientSceneConnector;
        private readonly ILifetimeCancellationToken _lifetimeCts;

        private const float _dockingTime = 3f;
        private const int _dockingAwaitDelay = 3200;

        public ServerDockingService(PlayerLocationManager playerLocationManager,
            ClientSceneConnector clientSceneConnector,
            ILifetimeCancellationToken lifetimeCts)
        {
            _playerLocationManager = playerLocationManager;
            _clientSceneConnector = clientSceneConnector;
            _lifetimeCts = lifetimeCts;
        }

        internal bool CharacterCanDock(int characterId, int stationId)
        {
            return true;
        }

        internal void StartDocking(int characterId, int stationId, int dockingBayId, ushort fromClientId)
        {
            DockingProcessAsync(characterId, stationId, dockingBayId, fromClientId, _lifetimeCts.Token).Forget();
        }

        private async UniTask DockingProcessAsync(int characterId, int stationId, int dockingBayId, ushort fromClientId, CancellationToken ct)
        {
            var playerEntity = _clientSceneConnector.GetEntityForPlayer(fromClientId);
            if (playerEntity != null)
            {
                playerEntity.StartDocking();                

                await UniTask.Delay(_dockingAwaitDelay);

                if (ct.IsCancellationRequested)
                    return;
            }

            _playerLocationManager.SetStationDocked(characterId, stationId, dockingBayId);
            _clientSceneConnector.ConnectPlayer(fromClientId);
        }
    }
}
