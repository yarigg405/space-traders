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
        private static string _sceneToConnect = string.Empty;

        private static NetworkManager _networkManager;


        public static void SetupDependencies(NetworkManager networkManager)
        {
            _networkManager = networkManager;
        }


        [MessageHandler((ushort)ServerToClientMessageType.ConnectToGameSceneCommand)]
        private static void HandleSceneConnectionCommand(Message message)
        {
            _sceneToConnect = message.GetString();
            Debug.Log($"<color=orange>### Need connect to scene {_sceneToConnect}");
        }

        public static async UniTask<string> RequestForConnectGame()
        {
            _sceneToConnect = string.Empty;
            Debug.Log("<color=orange>### Request for connect game()");
            var message = Message.Create(MessageSendMode.Reliable, ClientToServerMessageType.RequestConnectToGame);
            _networkManager.Client.Send(message);

            await UniTask.WaitWhile(() => _sceneToConnect.Length == 0)
                .AttachExternalCancellation(_cancellationTokenSource.Token); ;
            return _sceneToConnect;
        }
    }
}
