using Assets.Code.Gameplay.Common;
using Assets.Code.Gameplay.Features.Player.Factory;
using Assets.Code.Networking;
using System;
using System.Collections.Generic;
using Unity.Mathematics;
using VContainer.Unity;


namespace Assets.Code.Gameplay.Worlds
{
    internal sealed class ServerWorldsController : ITickable, IInitializable, IDisposable
    {
        private readonly ClientsScenesContainer _clientsScenesContainer;
        private readonly PlayerFactory _playerFactory;
        private readonly EcsWorldsBuilder _worldsBuilder;

        private readonly Dictionary<string, EcsWorldInstance> _scenesWorldsDict = new();
        private readonly Dictionary<ushort, GameEntity> _playersEntities = new();
        private readonly EcsWorldDestoyer _destoyer = new();


        public ServerWorldsController(ClientsScenesContainer clientsScenesContainer,
            PlayerFactory playerFactory,
            EcsWorldsBuilder worldsBuilder)
        {
            _clientsScenesContainer = clientsScenesContainer;
            _playerFactory = playerFactory;
            _worldsBuilder = worldsBuilder;
        }

        void IInitializable.Initialize()
        {
            _clientsScenesContainer.OnClientConnectedToScene += OnClientConnected;
            _clientsScenesContainer.OnClientDisconnectedFromScene += OnClientDisconnected;
        }

        void IDisposable.Dispose()
        {
            _clientsScenesContainer.OnClientConnectedToScene -= OnClientConnected;
            _clientsScenesContainer.OnClientDisconnectedFromScene -= OnClientDisconnected;
        }

        void ITickable.Tick()
        {
            foreach (var world in _scenesWorldsDict.Values)
            {
                world.Feature.Execute();
                world.Feature.Cleanup();
            }
        }

        private void OnClientConnected(string sceneName, ushort clientId)
        {
            if (!_scenesWorldsDict.ContainsKey(sceneName))
            {
                _scenesWorldsDict[sceneName] = _worldsBuilder.CreateNewServerWorld(sceneName);
            }

            var ctxs = _scenesWorldsDict[sceneName].Contexts;
            var spawnPoint = double2.zero.GetRandomCoordinatesAroundPointZX(25f);

            _playersEntities[clientId] =
                _playerFactory.CreatePlayer(clientId, sceneName, spawnPoint, ctxs);
        }

        private void OnClientDisconnected(string sceneName, ushort clientId)
        {
            _playersEntities[clientId].isDestructed = true;
            _playersEntities.Remove(clientId);
            if (_clientsScenesContainer.PlayersCount(sceneName) < 1)
            {
                _destoyer.DestroyWorld(_scenesWorldsDict[sceneName]);
                _scenesWorldsDict.Remove(sceneName);
            }
        }


    }
}
