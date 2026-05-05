namespace Assets.Code.ClientPart.Networking
{
    public enum ClientToServerMessageType : ushort
    {
        RequestGetCharacters = 1,
        RequestCreateCharacter = 2,

        RequestEnterTheGame = 3,
        RequestForSceneEntities = 4,
        RequestForChangeScene = 5,
        RequestForStationSceneData = 6,

        SendTargetRotation = 31,
        SendSpeedModifier = 32,
        SendKeepDistance = 33,
        SendSetOrbit = 34,
        SendSetWarpTo = 35,
    }
}
