
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
        private readonly ServerMessenger _messenger;

        internal EntitiesSynchronizator(string sceneName,
            ClientSceneConnector clientSceneConnector, ServerMessenger messenger)
        {
            _sceneName = sceneName;
            _clientSceneConnector = clientSceneConnector;
            _messenger = messenger;
        }

        public void SyncGlobalPosition(uint entityId, double2 globalPosition)
        {
            foreach (var client in _clientSceneConnector.GetClientsOnScene(_sceneName))
            {
                _messenger.SynchronizeGlobalPosition(client, entityId, globalPosition);
            }
        }

        public void SyncRotation(uint entityId, float rotation)
        {
            foreach (var client in _clientSceneConnector.GetClientsOnScene(_sceneName))
            {
                _messenger.SynchronizeRotation(client, entityId, rotation);
            }
        }

        public void UpdateComponentsForEntity(GameEntity entity, params int[] components)
        {
            var snapshot = entity.AsSerializedEntity(components);
            var json = JsonSerializator.ToJson(snapshot);

            foreach (var client in _clientSceneConnector.GetClientsOnScene(_sceneName))
            {
                _messenger.UpdateComponentsForEntity(client, entity.Id, json);
            }
        }
    }
}
