namespace Assets.Code.ClientPart.Networking
{
    public enum ClientToServerMessageType : ushort
    {
        RequestConnectToServer = 1,
        RequestGetCharacters = 2,
        RequestEnterTheGame = 3,
        RequestForSceneEntities = 4,
        RequestForChangeScene = 5,

        SendTargetRotation = 31,
        SendSpeedModifier = 32,
        SendKeepDistance = 33,
        SendSetOrbit = 34,
        SendSetWarpTo = 35,
    }
}
