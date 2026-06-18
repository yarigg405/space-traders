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
    public sealed class ClientSceneConnector : IInitializable, IDisposable
    {
        private readonly PlayerBuilder _playerBuilder;
        private readonly PlayerLocationManager _playerDataProvider;
        private readonly ServerWorldsController _serverWorldsController;
        private readonly NetworkManager _networkManager;
        private readonly ServerMessenger _messenger;
        private readonly PlayerSaver _playerSaver;
        private readonly PlayerCharacterManager _playerCharacterManager;

        private readonly Dictionary<ushort, string> _sceneForClientsMap = new();
        private readonly Dictionary<string, List<ushort>> _clientsOnScenesMap = new();
        private readonly Dictionary<ushort, GameEntity> _playerEntities = new();

        public ClientSceneConnector(PlayerBuilder playerBuilder,
            PlayerLocationManager playerDataProvider,
            ServerWorldsController serverWorldsController,
            NetworkManager networkManager,
            ServerMessenger messenger,
            PlayerSaver playerSaver,
            PlayerCharacterManager playerCharacterManager)
        {
            _playerBuilder = playerBuilder;
            _playerDataProvider = playerDataProvider;
            _serverWorldsController = serverWorldsController;

            _networkManager = networkManager;
            _messenger = messenger;
            _playerSaver = playerSaver;
            _playerCharacterManager = playerCharacterManager;
        }

        void IInitializable.Initialize()
        {
            _networkManager.Server.ClientDisconnected += OnClientDisconnected;
        }

        void IDisposable.Dispose()
        {
            foreach (var pair in _playerEntities)
            {
                _playerSaver.Save(pair.Key, pair.Value);
            }

            _networkManager.Server.ClientDisconnected -= OnClientDisconnected;
        }


        public void ConnectPlayer(ushort clientId)
        {
            TryDisconnectClientFromCurrentScene(clientId);
            var characterId = _playerCharacterManager.GetCharacterIdForPlayer(clientId);
            var sceneName = _playerDataProvider.GetSceneForCharacter(characterId);
            EnsureClientsListExist(sceneName);

            var newPlayerEntity = _playerBuilder.CreatePlayer(clientId);
            var snapshot = newPlayerEntity.AsSerializedEntity();
            foreach (var client in _clientsOnScenesMap[sceneName])
            {
                _messenger.SendEntityToClient(client, snapshot);
            }
            _playerEntities[clientId] = newPlayerEntity;

            AddClientToScene(clientId, sceneName);
        }

        private void OnClientDisconnected(object sender, ServerDisconnectedEventArgs e)
        {
            var clientId = e.Client.Id;
            var playerEntity = _playerEntities[clientId];
            _playerSaver.Save(clientId, playerEntity);

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
                _messenger.SendEntityToClient(clientId, snapshot);
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

            var entityId = _playerEntities[clientId].Id;
            foreach (var client in _clientsOnScenesMap[sceneName])
            {
                _messenger.DestroyEntityOnClient(client, entityId);
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

        public uint GetEntityIdForPlayer(ushort clientId)
        {
            if (_playerEntities.TryGetValue(clientId, out var entity))
                return entity.Id;

            return 0;
        }
    }
}
