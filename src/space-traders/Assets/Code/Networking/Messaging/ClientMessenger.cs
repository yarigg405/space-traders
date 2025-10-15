using Cysharp.Threading.Tasks;
using Riptide;
using UnityEngine;


namespace Assets.Code.Networking.Messaging
{
    public sealed class ClientMessenger
    {
        private static string _sceneToConnect = "";

        public async UniTask<string> RequestForConnectGame()
        {
            _sceneToConnect = string.Empty;
            Debug.Log("<color=orange>### Request for connect game()");
            var message = Message.Create(MessageSendMode.Reliable, ClientToServerMessage.RequestConnectToGameScene);
            NetworkManager.Client.Send(message);

            await UniTask.WaitWhile(() => _sceneToConnect.Length == 0);
            return _sceneToConnect;
        }

        [MessageHandler((ushort)ServerToClientMessage.ConnectToGameSceneCommand)]
        private static void HandleSceneConnectionCommand(Message message)
        {
            _sceneToConnect = message.GetString();
            Debug.Log($"<color=orange>### Need connect to scene {_sceneToConnect}");
        }
    }
}
