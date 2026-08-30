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

        [MessageHandler((ushort)ServerToClientMessageType.ResponseGetCharacters)]
        private static void HandleResponseGetCharacters(Message message)
        {
            _router.HandleResponseGetCharacters(message);
        }

        [MessageHandler((ushort)ServerToClientMessageType.ResponseCreateCharacter)]
        private static void HandleResponseCreateCharacter(Message message)
        {
            _router.HandleResponseCreateCharacter(message);
        }

        [MessageHandler((ushort)ServerToClientMessageType.ResponseEnterTheGame)]
        private static void HandleResponseEnterTheGame(Message message)
        {
            _router.HandleResponseEnterTheGame(message);
        }

        [MessageHandler((ushort)ServerToClientMessageType.ResponseLoadStationData)]
        private static void HandleResponseLoadStationData(Message message)
        {
            _router.HandleResponseLoadStationData(message);
        }

        [MessageHandler((ushort)ServerToClientMessageType.ResponseToUndock)]
        private static void HandleResponseToUndock(Message message)
        {
            _router.HandleResponseToUndock(message);
        }

        [MessageHandler((ushort)ServerToClientMessageType.ResponseToDock)]
        private static void HandleResponseToDock(Message message)
        {
            _router.HandleResponseToDock(message);
        }

        [MessageHandler((ushort)ServerToClientMessageType.ResponseMoney)]
        private static void HandleResponseMoney(Message message)
        {
            _router.HandleResponseMoney(message);
        }

        [MessageHandler((ushort)ServerToClientMessageType.ResponseItemOrders)]
        private static void HandleResponseItemOrders(Message message)
        {
            _router.HandleResponseItemOrders(message);
        }

        [MessageHandler((ushort)ServerToClientMessageType.ResponseBuyItem)]
        private static void HandleResponseBuyItem(Message message)
        {
            _router.HandleResponseBuyItem(message);
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

        [MessageHandler((ushort)ServerToClientMessageType.Pong)]
        private static void HandlePong(Message message)
        {
            _router.HandlePong(message);
        }

        [MessageHandler((ushort)ServerToClientMessageType.PlayerStateSnapshot)]
        private static void HandlePlayerStateSnapshot(Message message)
        {
            _router.HandlePlayerStateSnapshot(message);
        }

        [MessageHandler((ushort)ServerToClientMessageType.WorldSnapshot)]
        private static void HandleWorldSnapshot(Message message)
        {
            _router.HandleWorldSnapshot(message);
        }
    }
}
