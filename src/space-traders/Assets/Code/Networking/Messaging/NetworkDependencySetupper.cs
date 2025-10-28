using VContainer.Unity;


namespace Assets.Code.Networking.Messaging
{
    public sealed class NetworkDependencySetupper : IInitializable
    {
        private readonly NetworkManager _networkManager;
        private readonly ClientsScenesContainer _clientsScenesContainer;

        public NetworkDependencySetupper(NetworkManager networkManager,
            ClientsScenesContainer clientsScenesContainer)
        {
            _networkManager = networkManager;
            _clientsScenesContainer = clientsScenesContainer;
        }

        void IInitializable.Initialize()
        {
            ServerMessenger.SetupDependencies(_networkManager,
                _clientsScenesContainer);

            ClientMessenger.SetupDependencies(_networkManager);
        }
    }
}