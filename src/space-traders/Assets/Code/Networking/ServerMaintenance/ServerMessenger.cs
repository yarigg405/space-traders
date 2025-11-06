using Assets.Code.Networking.MessageTypes;
using Assets.Code.Serialization;
using Assets.Code.Serialization.Data;
using Riptide;
using VContainer;


namespace Assets.Code.Networking.ServerMaintenance
{
    public static class ServerMessenger
    {
        private static NetworkManager _networkManager;
        private static ClientSceneConnector _clientSceneConnector;

        public static void SetupDependencies(IObjectResolver resolver)
        {
            _networkManager = resolver.Resolve<NetworkManager>();
            _clientSceneConnector = resolver.Resolve<ClientSceneConnector>();
        }


        [MessageHandler((ushort)ClientToServerMessageType.RequestConnectToGame)]
        private static void HandleConnectToGameScene(ushort fromClientId, Message message)
        {
            _clientSceneConnector.ConnectPlayer(fromClientId);
        }

        public static void SendConnectionDataToPlayer(ushort clientId, string sceneName)
        {
            var message = Message.Create(MessageSendMode.Reliable, ServerToClientMessageType.ConnectToGameSceneCommand)
                .AddString(sceneName);

            _networkManager.Server.Send(message, clientId);
        }


        [MessageHandler((ushort)ClientToServerMessageType.RequestForSceneEntities)]
        private static void HandleEntitiesLoading(ushort fromClientId, Message message)
        {
            _clientSceneConnector.FillWorldForClient(fromClientId);
        }

        public static void SendEntityToClient(ushort clientId, EntitySnapshot snapshot)
        {
            var json = JsonSerializator.ToJson(snapshot);
            var message = Message.Create(MessageSendMode.Reliable, ServerToClientMessageType.CreateEntity)
                .AddString(json);

            _networkManager.Server.Send(message, clientId);
        }

        public static void DestroyEntityOnClient(ushort clientId,  uint entityId)
        {
            var message = Message.Create(MessageSendMode.Reliable, ServerToClientMessageType.DestroyEntity)
                .AddUInt(entityId);

            _networkManager.Server.Send(message, clientId);
        }            
    }
}
