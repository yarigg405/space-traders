using Assets.Code.Networking.ServerMaintenance;
using Cysharp.Threading.Tasks;
using Riptide;
using Riptide.Utils;
using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;


namespace Assets.Code.Networking
{
    public class NetworkManager : IInitializable, IDisposable, IFixedTickable
    {
        private const string _ipAddress = "127.0.0.1";
        private const ushort _port = 40501;
        private const ushort _maxPlayers = 4;

        private readonly IObjectResolver _resolver;

        public NetworkManager(IObjectResolver resolver)
        {
            _resolver = resolver;
        }

        private ServerStartup _serverStartup;

        public Server Server { get; private set; }
        public Client Client { get; private set; }

        public NetworkConnectionType ConnectionType { get; private set; }

        void IInitializable.Initialize()
        {
            RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);

            Server = new Server();
            Server.ClientConnected += PlayerJoined;

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
            _serverStartup.StopServer();

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


            _serverStartup = new(_resolver.Resolve<LifetimeScope>());

            ConnectionType = NetworkConnectionType.Host;
            Debug.Log("<color=#6BCCFF>### HOST started");
        }

        public async UniTask StartClient()
        {
            Client.Connect($"{_ipAddress}:{_port}");
            await UniTask.WaitUntil(() => Client.IsConnected);
            ConnectionType = NetworkConnectionType.Client;
            Debug.Log("<color=#6BCCFF>### CLIENT Connected");
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
    }

    public enum NetworkConnectionType
    {
        None = 0,
        Host = 1,
        Client = 2,
    }
}