using Assets.Code.Infrastructure.DI;
using Assets.Code.ServerPart.Networking;
using Cysharp.Threading.Tasks;
using Riptide;
using Riptide.Utils;
using System;
using System.Threading;
using UnityEngine;
using VContainer;
using VContainer.Unity;


namespace Assets.Code.Networking
{
    public class NetworkManager : IInitializable, IDisposable, IFixedTickable
    {
        private const ushort _maxPlayers = 6;
        private const string _hostIp = "127.0.0.1";

        private readonly IObjectResolver _resolver;

        private string _serverPassword;

        public NetworkManager(IObjectResolver resolver)
        {
            _resolver = resolver;
        }

        private ServerStartup _serverStartup;

        public Server Server { get; private set; }
        public Client Client { get; private set; }

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
            Cleanup();
        }

        public async UniTask StartHost(ushort port, string serverPassword, CancellationToken ct)
        {
            try
            {
                if (Server.IsRunning)
                    throw new InvalidOperationException("Server already running");

                if (Client.IsConnected)
                    throw new InvalidOperationException("Client already connected");

                _serverStartup = new(_resolver.Resolve<GameLifetimeScope>());

                Server.Start(port, _maxPlayers);
                await UniTask.WaitUntil(() => Server.IsRunning)
                        .AttachExternalCancellation(ct)
                        .Timeout(TimeSpan.FromSeconds(5));
                _serverPassword = serverPassword;

                Client.Connect($"{_hostIp}:{port}");
                await UniTask.WaitUntil(() => Client.IsConnected)
                        .AttachExternalCancellation(ct)
                        .Timeout(TimeSpan.FromSeconds(5));
            }
            catch (OperationCanceledException)
            {
                Debug.Log("StartHost cancelled");
                Cleanup();
                throw;
            }
            catch (TimeoutException)
            {
                Debug.LogError("StartHost timeout");
                Cleanup();
                throw;
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                Cleanup();
                throw;
            }
        }

        public async UniTask StartClient(string ipAddress, ushort port, CancellationToken ct)
        {
            try
            {
                if (Client.IsConnected)
                    throw new InvalidOperationException("Client already connected");

                Client.Connect($"{ipAddress}:{port}");
                await UniTask.WaitUntil(() => Client.IsConnected)
                        .AttachExternalCancellation(ct)
                        .Timeout(TimeSpan.FromSeconds(5));
            }
            catch (OperationCanceledException)
            {
                Debug.Log("StartClient cancelled");
                Cleanup();
                throw;
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                Cleanup();
                throw new("error-connection");
            }
        }

        public void Cleanup()
        {
            try
            {
                if (Client.IsConnected)
                    Client.Disconnect();

                if (_serverStartup != null)
                    _serverStartup.StopServer();

                if (Server.IsRunning)
                    Server.Stop();
            }

            catch (Exception ex)
            {
                Debug.LogError($"Cleanup failed: {ex}");
            }
        }

        public void StopClient()
        {
            if (Client.IsConnected)
                Client.Disconnect();
        }

        public bool CheckServerPassword(string password)
        {
            return _serverPassword.Equals(password);
        }
    }
}