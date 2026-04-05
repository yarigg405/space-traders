using Assets.Code.Common.Serialization;
using Assets.Code.Common.Serialization.Data;
using Assets.Code.Networking;
using Assets.Code.ServerPart.Networking;
using Cysharp.Threading.Tasks;
using Riptide;
using Unity.Mathematics;
using UnityEngine;
using VContainer;


namespace Assets.Code.ClientPart.Networking
{
    public static class ClientMessenger
    {
        private static NetworkManager _networkManager;
        private static ICancellationToken _cts;
        private static ClientEntitiesController _clientEntitiesController;

        private static string _sceneToConnect = string.Empty;


        public static void SetupDependencies(IObjectResolver resolver)
        {
            _networkManager = resolver.Resolve<NetworkManager>();
            _cts = resolver.Resolve<ICancellationToken>();
            _clientEntitiesController = resolver.Resolve<ClientEntitiesController>();
        }


        public static async UniTask<string> RequestForConnectGame()
        {
            _sceneToConnect = string.Empty;
            var message = Message.Create(MessageSendMode.Reliable, ClientToServerMessageType.RequestConnectToGame);
            _networkManager.Client.Send(message);

            await UniTask.WaitWhile(() => _sceneToConnect.Length == 0)
                .AttachExternalCancellation(_cts.Token);
            return _sceneToConnect;
        }

        public static void RequestForLoadingSceneEntities()
        {
            var message = Message.Create(MessageSendMode.Reliable, ClientToServerMessageType.RequestForSceneEntities);
            _networkManager.Client.Send(message);
        }

        public static void RequestForChangeScene(string sceneName)
        {
            var message = Message.Create(MessageSendMode.Reliable, ClientToServerMessageType.RequestForChangeScene)
                .AddString(sceneName);
            _networkManager.Client.Send(message);
        }

        public static void SendTargetRotationToServer(float targetRotation)
        {
            var message = Message.Create(MessageSendMode.Reliable, ClientToServerMessageType.SendTargetRotation)
                .AddFloat(targetRotation);
            _networkManager.Client.Send(message);
        }

        public static void SendSpeedModifierToServer(float speedModifier)
        {
            var message = Message.Create(MessageSendMode.Reliable, ClientToServerMessageType.SendSpeedModifier)
                .AddFloat(speedModifier);
            _networkManager.Client.Send(message);
        }

        public static void SendKeepDistance(uint targetId, Vector2 minMaxDistance)
        {
            var message = Message.Create(MessageSendMode.Reliable, ClientToServerMessageType.SendKeepDistance)
                .AddUInt(targetId)
                .AddVector2(minMaxDistance);
            _networkManager.Client.Send(message);
        }

        public static void SendSetOrbit(uint targetId, float orbitRadius)
        {
            var message = Message.Create(MessageSendMode.Reliable, ClientToServerMessageType.SendSetOrbit)
                 .AddUInt(targetId)
                 .AddFloat(orbitRadius);
            _networkManager.Client.Send(message);
        }

        public static void SendSetWarpTo(double2 coordinates)
        {
            var message = Message.Create(MessageSendMode.Reliable, ClientToServerMessageType.SendSetWarpTo)
                .AddDouble(coordinates.x)
                .AddDouble(coordinates.y);
            _networkManager.Client.Send(message);
        }



        #region MessageHandlers


        [MessageHandler((ushort)ServerToClientMessageType.ConnectToGameSceneCommand)]
        private static void HandleSceneConnectionCommand(Message message)
        {
            _sceneToConnect = message.GetString();
        }


        [MessageHandler((ushort)ServerToClientMessageType.CreateEntity)]
        private static void HandleCreateEntity(Message message)
        {
            var json = message.GetString();
            var snapshot = JsonSerializator.FromJson<EntitySnapshot>(json);

            _clientEntitiesController.CreateEntityFromSnapshot(snapshot);
        }

        [MessageHandler((ushort)ServerToClientMessageType.DestroyEntity)]
        private static void HandleDestroyEntity(Message message)
        {
            var entityId = message.GetUInt();
            _clientEntitiesController.DestroyEntity(entityId);
        }

        [MessageHandler((ushort)ServerToClientMessageType.SynchronizeGlobalPosition)]
        private static void HandleUpdateGlobalPosition(Message message)
        {
            var entityId = message.GetUInt();
            var x = message.GetDouble();
            var y = message.GetDouble();

            _clientEntitiesController.UpdateGlobalPosition(entityId, new double2(x, y));
        }

        [MessageHandler((ushort)ServerToClientMessageType.SynchronizeRotation)]
        private static void HandleUpdateRotation(Message message)
        {
            var entityId = message.GetUInt();
            var rotation = message.GetFloat();

            _clientEntitiesController.UpdateRotation(entityId, rotation);
        }

        [MessageHandler((ushort)ServerToClientMessageType.UpdateComponentsForEntity)]
        private static void HandleUpdateComponentsForEntity(Message message)
        {
            var entityId = message.GetUInt();
            var json = message.GetString();
            var snapshot = JsonSerializator.FromJson<EntitySnapshot>(json);

            _clientEntitiesController.UpdateEntityComponents(entityId, snapshot);
        }

        #endregion
    }
}
