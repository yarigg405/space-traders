using Assets.Code.ClientPart.Networking;
using Assets.Code.Common.Serialization;
using Assets.Code.Common.Serialization.Data;
using Assets.Code.Networking;
using Riptide;
using System;
using Unity.Mathematics;
using VContainer;


namespace Assets.Code.ServerPart.Networking
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

        public static void DestroyEntityOnClient(ushort clientId, uint entityId)
        {
            var message = Message.Create(MessageSendMode.Reliable, ServerToClientMessageType.DestroyEntity)
                .AddUInt(entityId);

            _networkManager.Server.Send(message, clientId);
        }



        internal static void SendGlobalPosition(ushort client, uint entityId, double2 globalPosition)
        {
            var message = Message.Create(MessageSendMode.Unreliable, ServerToClientMessageType.UpdateGlobalPosition)
                .AddUInt(entityId)
                .AddDouble(globalPosition.x)
                .AddDouble(globalPosition.y)
                ;

            _networkManager.Server.Send(message, client);
        }
    }
}
