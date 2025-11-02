namespace Assets.Code.Networking.MessageTypes
{
    public enum ClientToServerMessageType : ushort
    {
        RequestConnectToGame = 1,
        RequestForSceneEntities = 2,
    }
}
