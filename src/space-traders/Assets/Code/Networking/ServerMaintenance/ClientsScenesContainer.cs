using System;
using System.Collections.Generic;
using Yrr.Utils;


namespace Assets.Code.Networking.ServerMaintenance
{
    public sealed class ClientsScenesContainer
    {
        public event Action<string, ushort> OnClientConnectedToScene;
        public event Action<string, ushort> OnClientDisconnectedFromScene;

        private readonly Dictionary<ushort, string> _sceneForClientsMap = new();
        private readonly Dictionary<string, List<ushort>> _clientsOnScenesMap = new();


        public void ConnectClientToScene(ushort clientId, string sceneName)
        {
            TryDisconnectClientFromCurrentScene(clientId);
            SetSceneForClient(clientId, sceneName);
            AddClientToScene(clientId, sceneName);

            OnClientConnectedToScene?.Invoke(sceneName, clientId);
        }

        public void RemoveClientFromGame(ushort clientId)
        {
            TryDisconnectClientFromCurrentScene(clientId);
            _sceneForClientsMap.Remove(clientId);
        }

        public int PlayersCount(string sceneName)
        {
            return _clientsOnScenesMap[sceneName].Count;
        }


        private void TryDisconnectClientFromCurrentScene(ushort clientID)
        {
            if (!_sceneForClientsMap.ContainsKey(clientID)) return;
            if (_sceneForClientsMap[clientID].IsNulOrEmpty()) return;

            var sceneName = _sceneForClientsMap[clientID];
            _clientsOnScenesMap[sceneName].Remove(clientID);
            _sceneForClientsMap[clientID] = string.Empty;

            OnClientConnectedToScene?.Invoke(sceneName, clientID);
        }

        private void SetSceneForClient(ushort clientId, string sceneName)
        {
            _sceneForClientsMap[clientId] = sceneName;
        }

        private void AddClientToScene(ushort clientId, string sceneName)
        {
            if (!_clientsOnScenesMap.ContainsKey(sceneName))
                _clientsOnScenesMap[sceneName] = new List<ushort>();

            _clientsOnScenesMap[sceneName].Add(clientId);
        }
    }
}
