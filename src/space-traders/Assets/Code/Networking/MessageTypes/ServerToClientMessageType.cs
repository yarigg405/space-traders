namespace Assets.Code.Networking.MessageTypes
{
    public enum ServerToClientMessageType:ushort
    {
        ConnectToGameSceneCommand = 1,
        SendEntitiesJson = 2,
    }
}
