using Assets.Code.Gameplay.Worlds;
using Assets.Code.Infrastructure.Loading;
using Assets.Code.Networking.MessageTypes;
using Assets.Code.Serialization.Services;
using Riptide;
using UnityEngine;
using VContainer;


namespace Assets.Code.Networking.ServerMaintenance
{
    public static class ServerMessenger
    {
        private static NetworkManager _networkManager;
        private static ClientsScenesContainer _clientsScenesContainer;
        private static ServerWorldsController _serverWorldsController;
        private static WorldSerializationService _worldSerializationService;

        public static void SetupDependencies(IObjectResolver resolver)
        {
            _networkManager = resolver.Resolve<NetworkManager>();
            _clientsScenesContainer = resolver.Resolve<ClientsScenesContainer>();   
            _serverWorldsController = resolver.Resolve<ServerWorldsController>();
            _worldSerializationService = resolver.Resolve<WorldSerializationService>();
        }


        [MessageHandler((ushort)ClientToServerMessageType.RequestConnectToGame)]
        private static void HandleConnectToGameScene(ushort fromClientId, Message message)
        {
            var sceneName = SceneNames.GameScene1;

            _clientsScenesContainer.ConnectClientToScene(fromClientId, sceneName);
            ConnectPlayerToScene(fromClientId, sceneName);
        }

        private static void ConnectPlayerToScene(ushort clientId, string sceneName)
        {
            var message = Message.Create(MessageSendMode.Reliable, ServerToClientMessageType.ConnectToGameSceneCommand)
                .AddString(sceneName);

            _networkManager.Server.Send(message, clientId);
        }


        [MessageHandler((ushort)ClientToServerMessageType.RequestForSceneEntities)]
        private static void HandleEntitiesLoading(ushort fromClientId, Message message)
        {
            var clientScene = _clientsScenesContainer.GetSceneForClient(fromClientId);
            var world = _serverWorldsController.GetWorld(clientScene);
            var json = _worldSerializationService.SerializeGameWorld(world);

            SendJsonEntitiesForPlayer(fromClientId, json);
        }

        private static void SendJsonEntitiesForPlayer(ushort clientId, string json)
        {
            var message = Message.Create(MessageSendMode.Reliable, ServerToClientMessageType.SendEntitiesJson)
                 .AddString(json);

            _networkManager.Server.Send(message, clientId);
        }
    }
}
