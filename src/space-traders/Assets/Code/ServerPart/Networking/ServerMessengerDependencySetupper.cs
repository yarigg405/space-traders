using VContainer;
using VContainer.Unity;


namespace Assets.Code.ServerPart.Networking
{
    public sealed class ServerMessengerDependencySetupper : IInitializable
    {
        private readonly IObjectResolver _resolver;

        public ServerMessengerDependencySetupper(IObjectResolver resolver)
        {
            _resolver = resolver;
        }

        void IInitializable.Initialize()
        {
            ServerMessenger.SetupDependencies(_resolver);
        }
    }
}
