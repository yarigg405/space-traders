using System;
using System.Collections.Generic;
using UnityEngine;
using Yrr.Utils;


namespace Assets.Code.Networking.ServerMaintenance
{
    public sealed class ClientsScenesContainer
    {
        public event Action<string, ushort> OnClientConnectedToScene;
        public event Action<string, ushort> OnClientDisconnectedFromScene;

        private readonly Dictionary<ushort, string> _sceneForClientsMap = new();
        private readonly Dictionary<string, List<ushort>> _clientsOnScenesMap = new();

        private string _guid;
        public ClientsScenesContainer()
        {
            _guid = Guid.NewGuid().ToString();
            Debug.Log($"<color=green>{_guid}: {_sceneForClientsMap.Count}");
        }  

        public void ConnectClientToScene(ushort clientId, string sceneName)
        {
            Debug.Log($"<color=red>ConnectClientToScene {clientId}-{sceneName}");
            TryDisconnectClientFromCurrentScene(clientId);
            SetSceneForClient(clientId, sceneName);
            AddClientToScene(clientId, sceneName);

            OnClientConnectedToScene?.Invoke(sceneName, clientId);
            Debug.Log($"<color=green>{_guid}: {_sceneForClientsMap.Count}");
        }

        public void RemoveClientFromGame(ushort clientId)
        {
            TryDisconnectClientFromCurrentScene(clientId);
            _sceneForClientsMap.Remove(clientId);
            Debug.Log($"<color=green>{_guid}: {_sceneForClientsMap.Count}");
        }

        public int PlayersCount(string sceneName)
        {
            return _clientsOnScenesMap[sceneName].Count;
        }

        public string GetSceneForClient(ushort clientId)
        {
            Debug.Log($"<color=green>{_guid}: {_sceneForClientsMap.Count}");
            return _sceneForClientsMap[clientId];
        }



        private void TryDisconnectClientFromCurrentScene(ushort clientID)
        {
            if (!_sceneForClientsMap.ContainsKey(clientID)) return;
            if (_sceneForClientsMap[clientID].IsNulOrEmpty()) return;

            var sceneName = _sceneForClientsMap[clientID];
            _clientsOnScenesMap[sceneName].Remove(clientID);
            _sceneForClientsMap[clientID] = string.Empty;

            OnClientDisconnectedFromScene?.Invoke(sceneName, clientID);
            Debug.Log($"<color=green>{_guid}: {_sceneForClientsMap.Count}");
        }

        private void SetSceneForClient(ushort clientId, string sceneName)
        {
            _sceneForClientsMap[clientId] = sceneName;
            Debug.Log($"<color=green>{_guid}: {_sceneForClientsMap.Count}");
        }

        private void AddClientToScene(ushort clientId, string sceneName)
        {
            if (!_clientsOnScenesMap.ContainsKey(sceneName))
                _clientsOnScenesMap[sceneName] = new List<ushort>();

            _clientsOnScenesMap[sceneName].Add(clientId);
            Debug.Log($"<color=green>{_guid}: {_sceneForClientsMap.Count}");
        }
    }
}
