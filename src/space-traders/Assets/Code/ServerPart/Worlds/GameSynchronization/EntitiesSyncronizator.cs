using Assets.Code.ServerPart.Networking;
using Unity.Mathematics;


namespace Assets.Code.ServerPart.Worlds.GameSynchronization
{
    public sealed class EntitiesSyncronizator
    {
        private readonly string _sceneName;
        private readonly ClientSceneConnector _clientSceneConnector;

        internal EntitiesSyncronizator(string sceneName, ClientSceneConnector clientSceneConnector)
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
    }
}
