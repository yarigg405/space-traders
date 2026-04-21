using VContainer.Unity;


namespace Assets.Code.ServerPart.Networking
{
    public sealed class ServerMessengerDependencySetupper : IInitializable
    {
        private readonly ServerMessengerRouter _router;

        internal ServerMessengerDependencySetupper(ServerMessengerRouter router)
        {
            _router = router;
        }

        void IInitializable.Initialize()
        {
            ServerMessengerReceiver.Initialize(_router);
        }
    }
}
