namespace Assets.Code.ServerPart.Networking
{
    public enum ServerToClientMessageType : ushort
    {
        RequestFailed = 1,
        ResponseGetCharacters = 3,
        ResponseCreateCharacter = 4,
        ResponseEnterTheGame = 5,
        ResponseLoadStationData = 6,
        ResponseToUndock = 7,
        ResponseToDock = 8,

        CreateEntity = 50,
        DestroyEntity = 51,
        UpdateComponentsForEntity = 52,
        SynchronizeGlobalPosition = 53,
        SynchronizeRotation = 54,
    }
}
