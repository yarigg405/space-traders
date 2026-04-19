namespace Assets.Code.ClientPart.Networking
{
    public enum ClientToServerMessageType : ushort
    {
        RequestConnectToServer = 1,
        RequestGetCharacters = 2,
        RequestCreateCharacter = 3,

        RequestEnterTheGame = 4,
        RequestForSceneEntities = 5,
        RequestForChangeScene = 6,

        SendTargetRotation = 31,
        SendSpeedModifier = 32,
        SendKeepDistance = 33,
        SendSetOrbit = 34,
        SendSetWarpTo = 35,
    }
}
