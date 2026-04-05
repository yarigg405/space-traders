namespace Assets.Code.ServerPart.Networking
{
    public enum ServerToClientMessageType : ushort
    {
        RequestFailed = 1,
        ResponseConnectToServer = 2,
        ResponseGetCharacters = 3,
        ResponseEnterTheGame = 4,

        CreateEntity = 10,
        DestroyEntity = 11,

        UpdateComponentsForEntity = 30,
        SynchronizeGlobalPosition = 31,
        SynchronizeRotation = 32,
    }
}
