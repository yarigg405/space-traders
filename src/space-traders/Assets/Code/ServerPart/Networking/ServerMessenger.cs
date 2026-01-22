using Assets.Code.ClientPart.Networking;
using Assets.Code.Common.Serialization;
using Assets.Code.Common.Serialization.Data;
using Assets.Code.Networking;
using Assets.Code.ServerPart.Gameplay.Features.InputInteraction;
using Assets.Code.ServerPart.Gameplay.Features.Player.Infrastructure;
using DesperateDevs.Reflection;
using Riptide;
using Unity.Mathematics;
using UnityEngine;
using VContainer;


namespace Assets.Code.ServerPart.Networking
{
    public static class ServerMessenger
    {
        private static NetworkManager _networkManager;
        private static ClientSceneConnector _clientSceneConnector;
        private static PlayerDataProvider _playerDataProvider;
        private static ServerInputService _serverInputService;

        public static void SetupDependencies(IObjectResolver resolver)
        {
            _networkManager = resolver.Resolve<NetworkManager>();
            _clientSceneConnector = resolver.Resolve<ClientSceneConnector>();
            _playerDataProvider = resolver.Resolve<PlayerDataProvider>();
            _serverInputService = resolver.Resolve<ServerInputService>();
        }

        public static void SendConnectionDataToPlayer(ushort clientId, string sceneName)
        {
            var message = Message.Create(MessageSendMode.Reliable, ServerToClientMessageType.ConnectToGameSceneCommand)
                .AddString(sceneName);

            _networkManager.Server.Send(message, clientId);
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

        #region Update of components
        public static void SendGlobalPosition(ushort client, uint entityId, double2 globalPosition)
        {
            var message = Message.Create(MessageSendMode.Unreliable, ServerToClientMessageType.UpdateGlobalPosition)
                .AddUInt(entityId)
                .AddDouble(globalPosition.x)
                .AddDouble(globalPosition.y)
                ;

            _networkManager.Server.Send(message, client);
        }

        public static void SendRotation(ushort client, uint entityId, float currentRotation)
        {
            var message = Message.Create(MessageSendMode.Unreliable, ServerToClientMessageType.UpdateRotation)
                .AddUInt(entityId)
                .AddFloat(currentRotation)
                ;

            _networkManager.Server.Send(message, client);
        }

        #endregion



        #region MessageHandlers


        [MessageHandler((ushort)ClientToServerMessageType.RequestConnectToGame)]
        private static void HandleConnectToGameScene(ushort fromClientId, Message message)
        {
            _clientSceneConnector.ConnectPlayer(fromClientId);
        }


        [MessageHandler((ushort)ClientToServerMessageType.RequestForSceneEntities)]
        private static void HandleEntitiesLoading(ushort fromClientId, Message message)
        {
            _clientSceneConnector.FillWorldForClient(fromClientId);
        }

        [MessageHandler((ushort)ClientToServerMessageType.RequestForChangeScene)]
        private static void HandleChangeScene(ushort fromClientId, Message message)
        {
            var sceneName = message.GetString();
            var scene = _playerDataProvider.GetSceneNameForPlayer(fromClientId);
            if (scene.Equals(sceneName)) return;
            _playerDataProvider.SetPlayerScene(fromClientId, sceneName);

            _clientSceneConnector.ConnectPlayer(fromClientId);
        }

        [MessageHandler((ushort)ClientToServerMessageType.SendTargetRotation)]
        private static void HandleClientInput(ushort fromClientId, Message message)
        {
            var targetRotation = message.GetFloat();
            _serverInputService.SetPlayerTargetRotation(fromClientId, targetRotation);
        }

        #endregion
    }
}
