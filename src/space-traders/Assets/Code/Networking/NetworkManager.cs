using Cysharp.Threading.Tasks;
using Riptide;
using Riptide.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer.Unity;


namespace Assets.Code.Networking
{
    public class NetworkManager : IInitializable, IDisposable, IFixedTickable
    {
        private const string _ipAddress = "127.0.0.1";
        private const ushort _port = 40501;
        private const ushort _maxPlayers = 4;

        // sceneName, playerIds
        private readonly Dictionary<string, List<int>> _playerOnScenesMap = new();

        public Server Server { get; private set; }
        public Client Client { get; private set; }

        void IInitializable.Initialize()
        {
            RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);

            Server = new Server();
            Server.ClientConnected += PlayerJoined;
            Server.RelayFilter = new MessageRelayFilter(
                typeof(MessageId),
                MessageId.ConnectToGameScene,
                MessageId.DisconnectCurrentScene
                );

            Client = new Client();
            Client.Connected += DidConnect;
            Client.ConnectionFailed += FailedToConnect;
            Client.ClientDisconnected += PlayerLeft;
            Client.Disconnected += DidDisconnect;
        }

        void IFixedTickable.FixedTick()
        {
            if (Server.IsRunning)
                Server.Update();

            Client.Update();
        }

        void IDisposable.Dispose()
        {
            Server.Stop();

            Client.Connected -= DidConnect;
            Client.ConnectionFailed -= FailedToConnect;
            Client.ClientDisconnected -= PlayerLeft;
            Client.Disconnected -= DidDisconnect;
            Client.Disconnect();
        }

        public async UniTask StartHost()
        {
            Server.Start(_port, _maxPlayers);
            await UniTask.WaitUntil(() => Server.IsRunning);

            Client.Connect($"{_ipAddress}:{_port}");
            await UniTask.WaitUntil(() => Client.IsConnected);

            Debug.Log("<color=#6BCCFF>### HOST started");
        }

        public async UniTask StartClient()
        {
            Client.Connect($"{_ipAddress}:{_port}");
            await UniTask.WaitUntil(() => Client.IsConnected);
            Debug.Log("<color=#6BCCFF>### CLIENT started");
        }


        private void PlayerJoined(object sender, ServerConnectedEventArgs e)
        {
            Debug.Log("<color=#6BCCFF>### Player Joined");
        }

        private void DidConnect(object sender, EventArgs e)
        {
            Debug.Log("<color=#6BCCFF>### Did Connect");
        }

        private void FailedToConnect(object sender, ConnectionFailedEventArgs e)
        {
            Debug.Log("<color=#6BCCFF>### FailedToConnect");
        }

        private void DidDisconnect(object sender, DisconnectedEventArgs e)
        {
            Debug.Log("<color=#6BCCFF>### DidDisconnect");
        }

        private void PlayerLeft(object sender, ClientDisconnectedEventArgs e)
        {
            Debug.Log("<color=#6BCCFF>### PlayerLeft");
        }

        internal void ConnectToScene(string sceneName)
        {
            Debug.Log("### TryConnectToScene");
            var message = Message.Create(MessageSendMode.Reliable, MessageId.ConnectToGameScene)
                .AddUShort(Client.Id)
                .AddString(sceneName);
            Client.Send(message);
            Server.Send(message, Client.Id);
        }

        internal void DisconnectCurrentScene()
        {
            Debug.Log("### TryDisconnectCurrentScene");
            var message = Message.Create(MessageSendMode.Reliable, MessageId.DisconnectCurrentScene)
                .AddUShort(Client.Id);
            Client.Send(message);
            Server.Send(message, Client.Id);
        }

        [MessageHandler((ushort)MessageId.ConnectToGameScene)]
        private static void HandleConnectToGameScene(Message message)
        {
            var id = message.GetUShort();
            var sceneName = message.GetString();

            Debug.Log($"<color=yellow>### PLAYER {id} CONNECT to scene {sceneName}");
        }

        [MessageHandler((ushort)MessageId.DisconnectCurrentScene)]
        private static void HandleDisconnectCurrentScene(Message message)
        {
            var id = message.GetUShort();

            Debug.Log($"<color=yellow>### Player {id} Disconnect");
        }
    }
}