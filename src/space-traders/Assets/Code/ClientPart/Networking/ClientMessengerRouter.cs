using Assets.Code.Common.Serialization;
using Assets.Code.Common.Serialization.Data;
using Riptide;
using System;
using Unity.Mathematics;


namespace Assets.Code.ClientPart.Networking
{
    internal sealed class ClientMessengerRouter
    {
        private readonly NetworkRequestSystem _requestSystem;
        private readonly ClientEntitiesController _clientEntitiesController;

        public ClientMessengerRouter(NetworkRequestSystem requestSystem,
            ClientEntitiesController clientEntitiesController)
        {
            _requestSystem = requestSystem;
            _clientEntitiesController = clientEntitiesController;
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

        internal void HandleResponseStationTradeData(Message message)
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
    }
}
