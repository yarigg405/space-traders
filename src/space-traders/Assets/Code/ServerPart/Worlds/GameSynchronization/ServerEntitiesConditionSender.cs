using Assets.Code.Common.Serialization.Extensions;
using Assets.Code.ServerPart.Networking;


namespace Assets.Code.ServerPart.Worlds.GameSynchronization
{
    public sealed class ServerEntitiesConditionSender : IEntityCreator, IEntityDestroyer
    {
        private readonly string _sceneName;
        private readonly ClientSceneConnector _clientSceneConnector;
        private readonly ServerMessenger _messenger;

        internal ServerEntitiesConditionSender(string sceneName,
            ClientSceneConnector clientSceneConnector, ServerMessenger messenger)
        {
            _sceneName = sceneName;
            _clientSceneConnector = clientSceneConnector;
            _messenger = messenger;
        }

        void IEntityCreator.CreateEntityOnClients(GameEntity entity)
        {
            var snapshot = entity.AsSerializedEntity();
            foreach (var client in _clientSceneConnector.GetClientsOnScene(_sceneName))
            {
                _messenger.SendEntityToClient(client, snapshot);
            }
        }

        void IEntityDestroyer.DestroyEntityOnClients(uint entityId)
        {
            foreach (var client in _clientSceneConnector.GetClientsOnScene(_sceneName))
            {
                _messenger.DestroyEntityOnClient(client, entityId);
            }
        }
    }
}
