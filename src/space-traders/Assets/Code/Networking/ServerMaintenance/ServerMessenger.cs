using Assets.Code.Infrastructure.Loading;
using Assets.Code.Networking.MessageTypes;
using Riptide;
using UnityEngine;


namespace Assets.Code.Networking.ServerMaintenance
{
    public static class ServerMessenger
    {
        private static NetworkManager _networkManager;
        private static ClientsScenesContainer _clientsScenesContainer;

        public static void SetupDependencies(NetworkManager networkManager, ClientsScenesContainer clientsScenesContainer)
        {
            _networkManager = networkManager;
            _clientsScenesContainer = clientsScenesContainer;
        }


        [MessageHandler((ushort)ClientToServerMessageType.RequestConnectToGame)]
        private static void HandleConnectToGameScene(ushort fromClientId, Message message)
        {
            Debug.Log($"<color=yellow>### PLAYER {fromClientId} request CONNECT to game");
            var sceneName = SceneNames.GameScene1;

            _clientsScenesContainer.ConnectClientToScene(fromClientId, sceneName);
            ConnectPlayerToScene(fromClientId, sceneName);
        }

        private static void ConnectPlayerToScene(ushort clientId, string sceneName)
        {
            Debug.Log($"<color=yellow>### ConnectPlayerToScene {sceneName} ");
            var message = Message.Create(MessageSendMode.Reliable, ServerToClientMessageType.ConnectToGameSceneCommand)
                .AddString(sceneName);

            _networkManager.Server.Send(message, clientId);
        }
    }
}
