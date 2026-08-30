using Assets.Code.ClientPart.Gameplay.Features.Movement;
using Assets.Code.ClientPart.Gameplay.Features.Player.Infrastructure;
using Assets.Code.Common.Serialization;
using Assets.Code.Common.Serialization.Data;
using Assets.Code.Common.Time;
using Riptide;
using Unity.Mathematics;
using UnityEngine;


namespace Assets.Code.ClientPart.Networking
{
    internal sealed class ClientMessengerRouter
    {
        private readonly NetworkRequestSystem _requestSystem;
        private readonly ClientEntitiesController _clientEntitiesController;
        private readonly ClockSyncService _clockSync;
        private readonly PlayerReconciler _reconciler;
        private readonly RemoteSnapshotBuffer _remoteBuffer;

        public ClientMessengerRouter(NetworkRequestSystem requestSystem,
            ClientEntitiesController clientEntitiesController,
            ClockSyncService clockSync,
            PlayerReconciler reconciler,
            RemoteSnapshotBuffer remoteBuffer)
        {
            _requestSystem = requestSystem;
            _clientEntitiesController = clientEntitiesController;
            _clockSync = clockSync;
            _reconciler = reconciler;
            _remoteBuffer = remoteBuffer;
        }


        internal void HandleRequestFailed(Message message)
        {
            _requestSystem.SetResponseFailed(message);
        }

        internal void HandleResponseGetCharacters(Message message)
        {
            _requestSystem.SetResponseOk(message);
        }

        internal void HandleResponseCreateCharacter(Message message)
        {
            _requestSystem.SetResponseOk(message);
        }

        internal void HandleResponseEnterTheGame(Message message)
        {
            _requestSystem.SetResponseOk(message);
        }

        internal void HandleResponseLoadStationData(Message message)
        {
            _requestSystem.SetResponseOk(message);
        }

        internal void HandleResponseToUndock(Message message)
        {
            _requestSystem.SetResponseOk(message);
        }

        internal void HandleResponseToDock(Message message)
        {
            _requestSystem.SetResponseOk(message);
        }

        internal void HandleResponseMoney(Message message)
        {
            _requestSystem.SetResponseOk(message);
        }

        internal void HandleResponseItemOrders(Message message)
        {
            _requestSystem.SetResponseOk(message);
        }

        internal void HandleResponseBuyItem(Message message)
        {
            _requestSystem.SetResponseOk(message);
        }


        internal void HandleCreateEntity(Message message)
        {
            var json = message.GetString();
            var snapshot = JsonSerializator.FromJson<EntitySnapshot>(json);

            _clientEntitiesController.CreateEntityFromSnapshot(snapshot);
        }

        internal void HandleDestroyEntity(Message message)
        {
            var entityId = message.GetUInt();
            _clientEntitiesController.DestroyEntity(entityId);
        }

        internal void HandleUpdateGlobalPosition(Message message)
        {
            var entityId = message.GetUInt();
            var x = message.GetDouble();
            var y = message.GetDouble();

            _clientEntitiesController.UpdateGlobalPosition(entityId, new double2(x, y));
        }

        internal void HandleUpdateRotation(Message message)
        {
            var entityId = message.GetUInt();
            var rotation = message.GetFloat();

            _clientEntitiesController.UpdateRotation(entityId, rotation);
        }

        internal void HandleUpdateComponentsForEntity(Message message)
        {
            var entityId = message.GetUInt();
            var json = message.GetString();
            var snapshot = JsonSerializator.FromJson<EntitySnapshot>(json);

            _clientEntitiesController.UpdateEntityComponents(entityId, snapshot);
        }

        internal void HandlePong(Message message)
        {
            var pingId = message.GetUInt();
            var serverTick = message.GetUInt();

            _clockSync.OnPong(pingId, serverTick);
        }

        internal void HandlePlayerStateSnapshot(Message message)
        {
            var entityId = message.GetUInt();
            var serverTick = message.GetUInt();
            var pos = new double2(message.GetDouble(), message.GetDouble());
            var rotationY = message.GetFloat();
            var velocity = message.GetVector2();
            var moveSpeed = message.GetFloat();
            var targetRotation = message.GetFloat();
            var speedModifier = message.GetFloat();
            var isWarping = message.GetBool();

            _reconciler.OnSnapshot(entityId, serverTick, pos, rotationY, velocity, 
                moveSpeed, targetRotation, speedModifier, isWarping);
        }

        internal void HandleWorldSnapshot(Message message)
        {
            var serverTick = message.GetUInt();
            var count = message.GetInt();
            for (int i = 0; i < count; i++)
            {
                var id = message.GetUInt();
                var pos = new double2(message.GetDouble(), message.GetDouble());
                var rot = message.GetFloat();
                
                _remoteBuffer .Add(id,serverTick, pos, rot);
            }
            Debug.Log($"[WORLD] tick={serverTick} count={count}");
        }
    }
}
