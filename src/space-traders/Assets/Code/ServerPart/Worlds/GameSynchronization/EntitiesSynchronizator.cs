using Assets.Code.Common.Serialization;
using Assets.Code.Common.Serialization.Extensions;
using Assets.Code.ServerPart.Networking;
using Unity.Mathematics;


namespace Assets.Code.ServerPart.Worlds.GameSynchronization
{
    public sealed class EntitiesSynchronizator
    {
        private readonly string _sceneName;
        private readonly ClientSceneConnector _clientSceneConnector;

        internal EntitiesSynchronizator(string sceneName, ClientSceneConnector clientSceneConnector)
        {
            _sceneName = sceneName;
            _clientSceneConnector = clientSceneConnector;
        }

        public void SyncGlobalPosition(uint entityId, double2 globalPosition)
        {
            foreach (var client in _clientSceneConnector.GetClientsOnScene(_sceneName))
            {
                ServerMessenger.SynchronizeGlobalPosition(client, entityId, globalPosition);
            }
        }

        public void SyncRotation(uint entityId, float rotation)
        {
            foreach (var client in _clientSceneConnector.GetClientsOnScene(_sceneName))
            {
                ServerMessenger.SynchronizeRotation(client, entityId, rotation);
            }
        }

        public void UpdateComponentsForEntity(GameEntity entity, params int[] components)
        {
            var snapshot = entity.AsSerializedEntity(components);
            var json = JsonSerializator.ToJson(snapshot);

            foreach (var client in _clientSceneConnector.GetClientsOnScene(_sceneName))
            {
                ServerMessenger.UpdateComponentsForEntity(client, entity.Id, json);
            }
        }
    }
}
