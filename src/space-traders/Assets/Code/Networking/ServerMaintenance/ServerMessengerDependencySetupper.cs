using VContainer.Unity;


namespace Assets.Code.Networking.ServerMaintenance
{
    public sealed class ServerMessengerDependencySetupper : IInitializable
    {
        private readonly NetworkManager _networkManager;
        private readonly ClientsScenesContainer _clientsScenesContainer;

        public ServerMessengerDependencySetupper(ClientsScenesContainer clientsScenesContainer, NetworkManager networkManager)
        {
            _clientsScenesContainer = clientsScenesContainer;
            _networkManager = networkManager;
        }

        void IInitializable.Initialize()
        {
            ServerMessenger.SetupDependencies(_networkManager,
                _clientsScenesContainer);
        }
    }
}
