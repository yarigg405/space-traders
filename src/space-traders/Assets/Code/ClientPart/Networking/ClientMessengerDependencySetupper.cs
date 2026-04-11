using VContainer.Unity;


namespace Assets.Code.ClientPart.Networking
{
    public sealed class ClientMessengerDependencySetupper : IInitializable
    {
        private ClientMessengerRouter _router;

        internal ClientMessengerDependencySetupper(ClientMessengerRouter router)
        {
            _router = router;
        }

        void IInitializable.Initialize()
        {
            ClientMessengerReceiver.Initialize(_router);
        }
    }
}