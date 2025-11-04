using VContainer;
using VContainer.Unity;


namespace Assets.Code.Networking.ClientMaintenance
{
    public sealed class ClientMessengerDependencySetupper : IInitializable
    {
        private readonly IObjectResolver _resolver;

        public ClientMessengerDependencySetupper(IObjectResolver resolver)
        {
            _resolver = resolver;
        }

        void IInitializable.Initialize()
        {
            ClientMessenger.SetupDependencies(_resolver);
        }
    }
}