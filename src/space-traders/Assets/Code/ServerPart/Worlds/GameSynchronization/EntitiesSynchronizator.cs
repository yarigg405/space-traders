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
                ServerMessenger.SendGlobalPosition(client, entityId, globalPosition);
            }
        }

        public void SyncRotation(uint entityId, float rotation)
        {
            foreach (var client in _clientSceneConnector.GetClientsOnScene(_sceneName))
            {
                ServerMessenger.SendRotation(client, entityId, rotation);
            }
        }
    }
}
