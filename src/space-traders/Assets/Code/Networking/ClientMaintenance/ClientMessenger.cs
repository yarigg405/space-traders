using Assets.Code.Networking.MessageTypes;
using Cysharp.Threading.Tasks;
using Riptide;
using System.Threading;
using UnityEngine;


namespace Assets.Code.Networking.ClientMaintenance
{
    public static class ClientMessenger
    {
        private static CancellationTokenSource _cancellationTokenSource = new();
        private static NetworkManager _networkManager;

        private static string _sceneToConnect = string.Empty;
        private static string _jsonToSceneLoading = string.Empty;


        public static void SetupDependencies(NetworkManager networkManager)
        {
            _networkManager = networkManager;
        }


        [MessageHandler((ushort)ServerToClientMessageType.ConnectToGameSceneCommand)]
        private static void HandleSceneConnectionCommand(Message message)
        {
            _sceneToConnect = message.GetString();
        }

        public static async UniTask<string> RequestForConnectGame()
        {
            _sceneToConnect = string.Empty;
            var message = Message.Create(MessageSendMode.Reliable, ClientToServerMessageType.RequestConnectToGame);
            _networkManager.Client.Send(message);

            await UniTask.WaitWhile(() => _sceneToConnect.Length == 0)
                .AttachExternalCancellation(_cancellationTokenSource.Token);
            return _sceneToConnect;
        }


        [MessageHandler((ushort)ServerToClientMessageType.SendEntitiesJson)]
        private static void HandleEntitiesJsonLoading(Message message)
        {
            _jsonToSceneLoading = message.GetString();
        }

        public static async UniTask<string> RequestForLoadingSceneEntities()
        {
            _jsonToSceneLoading = string.Empty;
            var message = Message.Create(MessageSendMode.Reliable, ClientToServerMessageType.RequestForSceneEntities);
            _networkManager.Client.Send(message);

            await UniTask.WaitWhile(() => _jsonToSceneLoading.Length == 0)
                   .AttachExternalCancellation(_cancellationTokenSource.Token);
            return _jsonToSceneLoading;
        }
    }
}
