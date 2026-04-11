using Assets.Code.Networking;
using Cysharp.Threading.Tasks;
using Riptide;
using System;
using System.Threading;
using Unity.Mathematics;
using UnityEngine;


namespace Assets.Code.ClientPart.Networking
{
    public sealed class ClientMessenger
    {
        private readonly NetworkRequestSystem _requestSystem;
        private readonly NetworkManager _networkManager;

        internal ClientMessenger(NetworkRequestSystem requestSystem, NetworkManager networkManager)
        {
            _requestSystem = requestSystem;
            _networkManager = networkManager;
        }


        public async UniTask<string> RequestForEnterTheGame(CancellationToken ct)
        {
            var msg = Message.Create(MessageSendMode.Reliable, ClientToServerMessageType.RequestEnterTheGame);

            var response = await _requestSystem.SendRequest(
                msg, ct, TimeSpan.FromSeconds(5));

            return response.GetString();
        }

        public void RequestForLoadingSceneEntities()
        {
            var message = Message.Create(MessageSendMode.Reliable, ClientToServerMessageType.RequestForSceneEntities);
            _networkManager.Client.Send(message);
        }

        public void RequestForChangeScene(string sceneName)
        {
            var message = Message.Create(MessageSendMode.Reliable, ClientToServerMessageType.RequestForChangeScene)
                .AddString(sceneName);
            _networkManager.Client.Send(message);
        }

        public void SendTargetRotationToServer(float targetRotation)
        {
            var message = Message.Create(MessageSendMode.Reliable, ClientToServerMessageType.SendTargetRotation)
                .AddFloat(targetRotation);
            _networkManager.Client.Send(message);
        }

        public void SendSpeedModifierToServer(float speedModifier)
        {
            var message = Message.Create(MessageSendMode.Reliable, ClientToServerMessageType.SendSpeedModifier)
                .AddFloat(speedModifier);
            _networkManager.Client.Send(message);
        }

        public void SendKeepDistance(uint targetId, Vector2 minMaxDistance)
        {
            var message = Message.Create(MessageSendMode.Reliable, ClientToServerMessageType.SendKeepDistance)
                .AddUInt(targetId)
                .AddVector2(minMaxDistance);
            _networkManager.Client.Send(message);
        }

        public void SendSetOrbit(uint targetId, float orbitRadius)
        {
            var message = Message.Create(MessageSendMode.Reliable, ClientToServerMessageType.SendSetOrbit)
                 .AddUInt(targetId)
                 .AddFloat(orbitRadius);
            _networkManager.Client.Send(message);
        }

        public void SendSetWarpTo(double2 coordinates)
        {
            var message = Message.Create(MessageSendMode.Reliable, ClientToServerMessageType.SendSetWarpTo)
                .AddDouble(coordinates.x)
                .AddDouble(coordinates.y);
            _networkManager.Client.Send(message);
        }
    }
}
