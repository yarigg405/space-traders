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



        //[MessageHandler((ushort)ClientToServerMessageType.RequestForSceneEntities)]
        //private static void HandleEntitiesLoading(ushort fromClientId, Message message)
        //{
        //    _clientSceneConnector.FillWorldForClient(fromClientId);
        //}

        //[MessageHandler((ushort)ClientToServerMessageType.RequestForChangeScene)]
        //private static void HandleChangeScene(ushort fromClientId, Message message)
        //{
        //    var sceneName = message.GetString();
        //    var scene = _playerDataProvider.GetSceneNameForPlayer(fromClientId);
        //    if (scene.Equals(sceneName)) return;
        //    _playerDataProvider.SetPlayerScene(fromClientId, sceneName);

        //    _clientSceneConnector.ConnectPlayer(fromClientId);
        //}

        [MessageHandler((ushort)ClientToServerMessageType.SendTargetRotation)]
        private static void HandleClientTargetRotation(ushort fromClientId, Message message)
        {
            _router.HandleClientTargetRotation(fromClientId, message);
        }

        [MessageHandler((ushort)ClientToServerMessageType.SendSpeedModifier)]
        private static void HandleClientSpeedModifier(ushort fromClientId, Message message)
        {
            _router.HandleClientSpeedModifier(fromClientId, message);
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
    }
}
