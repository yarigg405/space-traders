using Assets.Code.Infrastructure.DI;
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

        public event Action<ushort> OnClientDisconnected;

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

            Server = new();
            Client = new();
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
            if (_serverStartup != null)
                _serverStartup.StopServer();

            Client.Disconnect();
        }

        public async UniTask StartHost()
        {
            Server.Start(_port, _maxPlayers);
            await UniTask.WaitUntil(() => Server.IsRunning);

            Client.Connect($"{_ipAddress}:{_port}");
            await UniTask.WaitUntil(() => Client.IsConnected);


            _serverStartup = new(_resolver.Resolve<GameLifetimeScope>());

            ConnectionType = NetworkConnectionType.Host;
        }

        public async UniTask StartClient()
        {
            Client.Connect($"{_ipAddress}:{_port}");
            await UniTask.WaitUntil(() => Client.IsConnected);
            ConnectionType = NetworkConnectionType.Client;
        }
    }

    public enum NetworkConnectionType
    {
        None = 0,
        Host = 1,
        Client = 2,
    }
}