using Assets.Code.ServerPart.Networking;
using Riptide;


namespace Assets.Code.ClientPart.Networking
{
    internal static class ClientMessengerReceiver
    {
        private static ClientMessengerRouter _router;

        internal static void Initialize(ClientMessengerRouter clientRouter)
        {
            _router = clientRouter;
        }


        [MessageHandler((ushort)ServerToClientMessageType.RequestFailed)]
        private static void HandleRequestFailed(Message message)
        {
            _router.HandleRequestFailed(message);
        }

        [MessageHandler((ushort)ServerToClientMessageType.ResponseConnectToServer)]
        private static void HandleResponseConnectToServer(Message message)
        {
            _router.HandleResponseConnectToServer(message);
        }

        [MessageHandler((ushort)ServerToClientMessageType.ResponseGetCharacters)]
        private static void HandleResponseGetCharacters(Message message)
        {
            _router.HandleResponseGetCharacters(message);
        }

        [MessageHandler((ushort)ServerToClientMessageType.ResponseEnterTheGame)]
        private static void HandleResponseEnterTheGame(Message message)
        {
            _router.HandleResponseEnterTheGame(message);
        }


        [MessageHandler((ushort)ServerToClientMessageType.CreateEntity)]
        private static void HandleCreateEntity(Message message)
        {
            _router.HandleCreateEntity(message);
        }

        [MessageHandler((ushort)ServerToClientMessageType.DestroyEntity)]
        private static void HandleDestroyEntity(Message message)
        {
            _router.HandleDestroyEntity(message);
        }

        [MessageHandler((ushort)ServerToClientMessageType.SynchronizeGlobalPosition)]
        private static void HandleUpdateGlobalPosition(Message message)
        {
            _router.HandleUpdateGlobalPosition(message);
        }

        [MessageHandler((ushort)ServerToClientMessageType.SynchronizeRotation)]
        private static void HandleUpdateRotation(Message message)
        {
            _router.HandleUpdateRotation(message);
        }

        [MessageHandler((ushort)ServerToClientMessageType.UpdateComponentsForEntity)]
        private static void HandleUpdateComponentsForEntity(Message message)
        {
            _router.HandleUpdateComponentsForEntity(message);
        }
    }
}
