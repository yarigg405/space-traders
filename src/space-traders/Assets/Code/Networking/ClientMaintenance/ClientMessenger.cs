using Assets.Code.Common.Entity;
using Assets.Code.Networking.MessageTypes;
using Assets.Code.Serialization;
using Assets.Code.Serialization.Data;
using Cysharp.Threading.Tasks;
using Riptide;
using System.Threading;
using UnityEngine;
using VContainer;


namespace Assets.Code.Networking.ClientMaintenance
{
    public static class ClientMessenger
    {
        private static CancellationTokenSource _cancellationTokenSource = new();
        private static NetworkManager _networkManager;
        private static ClientEntitiesController _clientEntitiesController;

        private static string _sceneToConnect = string.Empty;


        public static void SetupDependencies(IObjectResolver resolver)
        {
            _networkManager = resolver.Resolve<NetworkManager>();
            _clientEntitiesController = resolver.Resolve<ClientEntitiesController>();
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


        [MessageHandler((ushort)ServerToClientMessageType.CreateEntity)]
        private static void HandleEntitiesJsonLoading(Message message)
        {
            var json = message.GetString();
            var snapshot = JsonSerializator.FromJson<EntitySnapshot>(json);

            _clientEntitiesController.CreateEntityFromSnapshot(snapshot);
        }

        public static void RequestForLoadingSceneEntities()
        {
            var message = Message.Create(MessageSendMode.Reliable, ClientToServerMessageType.RequestForSceneEntities);
            _networkManager.Client.Send(message);
        }
    }
}
