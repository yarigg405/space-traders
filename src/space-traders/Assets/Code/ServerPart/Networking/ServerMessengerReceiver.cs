using Assets.Code.ClientPart.Networking;
using Riptide;


namespace Assets.Code.ServerPart.Networking
{
    internal static class ServerMessengerReceiver
    {
        private static ServerMessengerRouter _router;

        internal static void Initialize(ServerMessengerRouter router)
        {
            _router = router;
        }


        [MessageHandler((ushort)ClientToServerMessageType.RequestGetCharacters)]
        private static void HandleRequestGetCharacters(ushort fromClientId, Message message)
        {
            _router.HandleRequestGetCharacters(fromClientId, message);
        }

        [MessageHandler((ushort)ClientToServerMessageType.RequestCreateCharacter)]
        private static void HandleRequestCreateCharacter(ushort fromClientId, Message message)
        {
            _router.HandleRequestCreateCharacter(fromClientId, message);
        }

        [MessageHandler((ushort)ClientToServerMessageType.RequestEnterTheGame)]
        private static void HandleRequestEnterTheGame(ushort fromClientId, Message message)
        {
            _router.HandleRequestEnterTheGame(fromClientId, message);
        }

        [MessageHandler((ushort)ClientToServerMessageType.RequestForStationSceneData)]
        private static void HandleRequestForStationSceneData(ushort fromClientId, Message message)
        {
            _router.HandleRequestForStationSceneData(fromClientId, message);
        }

        [MessageHandler((ushort)ClientToServerMessageType.RequestForUndock)]
        private static void HandleRequestForUndock(ushort fromClientId, Message message)
        {
            _router.HandleRequestForUndock(fromClientId, message);
        }

        [MessageHandler((ushort) ClientToServerMessageType.RequestForDock)]
        private static void HandleRequestForDock(ushort fromClientId, Message message)
        {
            _router.HandleRequestForDock(fromClientId, message);
        }

        [MessageHandler((ushort)ClientToServerMessageType.RequestForMoney)]
        private static void HandleRequestForMoney(ushort fromClientId, Message message)
        {
            _router.HandleRequestForMoney(fromClientId, message);
        }

        [MessageHandler((ushort)ClientToServerMessageType.RequestItemOrders)]
        private static void HandleRequestItemOrders(ushort fromClientId, Message message)
        {
            _router.HandleRequestItemOrders(fromClientId, message);
        }

        [MessageHandler((ushort)ClientToServerMessageType.RequestBuyItem)]
        private static void HandleRequestBuyItem(ushort fromClientId, Message message)
        {
            _router.HandleRequestBuyItem(fromClientId, message);
        }


        [MessageHandler((ushort)ClientToServerMessageType.RequestForSceneEntities)]
        private static void HandleEntitiesLoading(ushort fromClientId, Message message)
        {
            _router.HandleEntitiesLoading(fromClientId, message);
        }        

        [MessageHandler((ushort)ClientToServerMessageType.SendKeepDistance)]
        private static void HandleKeepDistance(ushort fromClientId, Message message)
        {
            _router.HandleKeepDistance(fromClientId, message);
        }

        [MessageHandler((ushort)ClientToServerMessageType.SendSetOrbit)]
        private static void HandleSetOrbig(ushort fromClientId, Message message)
        {
            _router.HandleSetOrbig(fromClientId, message);
        }

        [MessageHandler((ushort)ClientToServerMessageType.SendSetWarpTo)]
        private static void HandleSetWarpTo(ushort fromClientId, Message message)
        {
            _router.HandleSetWarpTo(fromClientId, message);
        }

        [MessageHandler((ushort)ClientToServerMessageType.SendMoveInput)]
        private static void HandeMoveInput(ushort fromClientId, Message message)
        {
            _router.HandleMoveInput(fromClientId, message);
        }
    }
}
