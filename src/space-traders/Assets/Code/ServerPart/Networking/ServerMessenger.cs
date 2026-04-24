using Assets.Code.Common.Serialization;
using Assets.Code.Common.Serialization.Data;
using Assets.Code.Networking;
using Riptide;
using Unity.Mathematics;


namespace Assets.Code.ServerPart.Networking
{
    public class ServerMessenger
    {
        private readonly NetworkManager _networkManager;

        public ServerMessenger(NetworkManager networkManager)
        {
            _networkManager = networkManager;
        }

        public void SendEntityToClient(ushort clientId, EntitySnapshot snapshot)
        {
            var json = JsonSerializator.ToJson(snapshot);
            var message = Message.Create(MessageSendMode.Reliable, ServerToClientMessageType.CreateEntity)
                .AddString(json);

            _networkManager.Server.Send(message, clientId);
        }

        public void DestroyEntityOnClient(ushort clientId, uint entityId)
        {
            var message = Message.Create(MessageSendMode.Reliable, ServerToClientMessageType.DestroyEntity)
                .AddUInt(entityId);

            _networkManager.Server.Send(message, clientId);
        }

        #region Update of components
        public void SynchronizeGlobalPosition(ushort client, uint entityId, double2 globalPosition)
        {
            var message = Message.Create(MessageSendMode.Unreliable, ServerToClientMessageType.SynchronizeGlobalPosition)
                .AddUInt(entityId)
                .AddDouble(globalPosition.x)
                .AddDouble(globalPosition.y)
                ;

            _networkManager.Server.Send(message, client);
        }

        public void SynchronizeRotation(ushort client, uint entityId, float currentRotation)
        {
            var message = Message.Create(MessageSendMode.Unreliable, ServerToClientMessageType.SynchronizeRotation)
                .AddUInt(entityId)
                .AddFloat(currentRotation)
                ;

            _networkManager.Server.Send(message, client);
        }

        public void UpdateComponentsForEntity(ushort client, uint entityId, string snapshotJson)
        {
            var message = Message.Create(MessageSendMode.Reliable, ServerToClientMessageType.UpdateComponentsForEntity)
                .AddUInt(entityId)
                .AddString(snapshotJson);

            _networkManager.Server.Send(message, client);
        }

        #endregion
    }
}
