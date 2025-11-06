using Assets.Code.Networking.ServerMaintenance;
using Assets.Code.Serialization.Extensions;


namespace Assets.Code.Gameplay.Worlds.GameSynchronization
{
    public sealed class ServerEntitiesConditionSender : IEntityCreator, IEntityDestroyer
    {
        private readonly string _sceneName;
        private readonly ClientSceneConnector _clientSceneConnector;

        internal ServerEntitiesConditionSender(string sceneName, ClientSceneConnector clientSceneConnector)
        {
            _sceneName = sceneName;
            _clientSceneConnector = clientSceneConnector;
        }

        void IEntityCreator.CreateEntityOnClients(GameEntity entity)
        {
            var snapshot = entity.AsSerializedEntity();
            foreach (var client in _clientSceneConnector.GetClientsOnScene(_sceneName))
            {
                ServerMessenger.SendEntityToClient(client, snapshot);
            }
        }

        void IEntityDestroyer.DestroyEntityOnClients(uint entityId)
        {
            foreach (var client in _clientSceneConnector.GetClientsOnScene(_sceneName))
            {
                ServerMessenger.DestroyEntityOnClient(client, entityId);
            }
        }
    }
}
