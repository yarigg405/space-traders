using VContainer.Unity;


namespace Assets.Code.Networking.ClientMaintenance
{
    public sealed class ClientMessengerDependencySetupper : IInitializable
    {
        private readonly NetworkManager _networkManager;

        public ClientMessengerDependencySetupper(NetworkManager networkManager)
        {
            _networkManager = networkManager;
        }

        void IInitializable.Initialize()
        {
            ClientMessenger.SetupDependencies(_networkManager);
        }
    }
}