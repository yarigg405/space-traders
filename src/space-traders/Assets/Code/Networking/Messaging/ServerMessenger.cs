using Assets.Code.Infrastructure.Loading;
using Riptide;
using UnityEngine;


namespace Assets.Code.Networking.Messaging
{
    public sealed class ServerMessenger
    {
        private readonly NetworkManager _networkManager;

        public ServerMessenger(NetworkManager networkManager)
        {
            _networkManager = networkManager;
        }

        [MessageHandler((ushort)ClientToServerMessage.RequestConnectToGameScene)]
        private static void HandleConnectToGameScene(ushort fromClientId, Message message)
        {
            Debug.Log($"<color=yellow>### PLAYER {fromClientId} request CONNECT to scene ");

            ConnectPlayerToScene(fromClientId, SceneNames.GameScene1);
        }

        private static void ConnectPlayerToScene(ushort clientId, string sceneName)
        {
            Debug.Log($"<color=yellow>### ConnectPlayerToScene {sceneName} ");
            var message = Message.Create(MessageSendMode.Reliable, ServerToClientMessage.ConnectToGameSceneCommand)
                .AddString(sceneName);

            NetworkManager.Server.Send(message, clientId);
        }
    }
}
