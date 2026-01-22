namespace Assets.Code.ClientPart.Networking
{
    public enum ClientToServerMessageType : ushort
    {
        RequestConnectToGame = 1,
        RequestForSceneEntities = 2,
        RequestForChangeScene = 3,

        SendTargetRotation = 31,
        SendSpeedModifier = 32,
    }
}
