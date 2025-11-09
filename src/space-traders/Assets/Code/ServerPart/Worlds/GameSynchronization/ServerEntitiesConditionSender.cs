using Assets.Code.Common.Serialization.Extensions;
using Assets.Code.ServerPart.Networking;


namespace Assets.Code.ServerPart.Worlds.GameSynchronization
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
