using Assets.Code.Common.Serialization;
using Assets.Code.Common.Serialization.Extensions;
using Assets.Code.Networking;
using Assets.Code.ServerPart.Gameplay.Features.Player.Infrastructure;
using Assets.Code.ServerPart.Worlds;
using Riptide;
using System;
using System.Collections.Generic;
using System.Linq;
using VContainer.Unity;
using Yrr.Utils;


namespace Assets.Code.ServerPart.Networking
{
    internal class ClientSceneConnector : IInitializable, IDisposable
    {
        private readonly PlayerBuilder _playerBuilder;
        private readonly PlayerDataProvider _playerDataProvider;
        private readonly ServerWorldsController _serverWorldsController;
        private readonly NetworkManager _networkManager;

        private readonly Dictionary<ushort, string> _sceneForClientsMap = new();
        private readonly Dictionary<string, List<ushort>> _clientsOnScenesMap = new();
        private readonly Dictionary<ushort, GameEntity> _playerEntities = new();

        public ClientSceneConnector(PlayerBuilder playerBuilder,
            PlayerDataProvider playerDataProvider,
            ServerWorldsController serverWorldsController,
            NetworkManager networkManager)
        {
            _playerBuilder = playerBuilder;
            _playerDataProvider = playerDataProvider;
            _serverWorldsController = serverWorldsController;

            _networkManager = networkManager;
        }

        void IInitializable.Initialize()
        {
            _networkManager.Server.ClientDisconnected += OnClientDisconnected;
        }

        void IDisposable.Dispose()
        {
            _networkManager.Server.ClientDisconnected -= OnClientDisconnected;
        }


        public void ConnectPlayer(ushort clientId)
        {
            TryDisconnectClientFromCurrentScene(clientId);
            var sceneName = _playerDataProvider.GetSceneNameForPlayer(clientId);
            EnsureClientsListExist(sceneName);

            var newPlayerEntity = _playerBuilder.CreatePlayer(clientId);
            var snapshot = newPlayerEntity.AsSerializedEntity();
            foreach (var client in _clientsOnScenesMap[sceneName])
            {
                ServerMessenger.SendEntityToClient(client, snapshot);
            }
            _playerEntities[clientId] = newPlayerEntity;

            AddClientToScene(clientId, sceneName);
            ServerMessenger.SendConnectionDataToPlayer(clientId, sceneName);
        }

        private void OnClientDisconnected(object sender, ServerDisconnectedEventArgs e)
        {
            var clientId = e.Client.Id;
            TryDisconnectClientFromCurrentScene(clientId);
            _sceneForClientsMap.Remove(clientId);
        }

        public void FillWorldForClient(ushort clientId)
        {
            var clientScene = _sceneForClientsMap[clientId];
            var world = _serverWorldsController.GetOrCreateWorld(clientScene);

            var snapshots = world.Contexts.game.GetEntities()
                  .Where(x => x.GetComponents().Any(c => c is ISerializeComponent))
                  .Select(e => e.AsSerializedEntity())
                  .ToList();

            foreach (var snapshot in snapshots)
            {
                ServerMessenger.SendEntityToClient(clientId, snapshot);
            }
        }

        public IEnumerable<ushort> GetClientsOnScene(string sceneName)
        {
            if (_clientsOnScenesMap.TryGetValue(sceneName, out var clients))
                return clients;

            return Enumerable.Empty<ushort>();
        }

        private void EnsureClientsListExist(string sceneName)
        {
            if (!_clientsOnScenesMap.ContainsKey(sceneName))
            {
                _clientsOnScenesMap[sceneName] = new();
            }
        }


        private void TryDisconnectClientFromCurrentScene(ushort clientId)
        {
            if (!_sceneForClientsMap.ContainsKey(clientId)) return;
            if (_sceneForClientsMap[clientId].IsNulOrEmpty()) return;

            var sceneName = _sceneForClientsMap[clientId];
            _clientsOnScenesMap[sceneName].Remove(clientId);
            if (_clientsOnScenesMap[sceneName].Count < 1)
                _serverWorldsController.DestroyWorld(sceneName);

            var entityId = _playerEntities[clientId].Id;
            foreach (var client in _clientsOnScenesMap[sceneName])
            {
                ServerMessenger.DestroyEntityOnClient(client, entityId);
            }

            _sceneForClientsMap[clientId] = string.Empty;
            _playerEntities[clientId].isDestructed = true;
            _playerEntities.Remove(clientId);
        }

        private void AddClientToScene(ushort clientId, string sceneName)
        {
            _sceneForClientsMap[clientId] = sceneName;
            _clientsOnScenesMap[sceneName].Add(clientId);
        }
    }
}
