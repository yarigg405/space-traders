namespace Assets.Code.ServerPart.Networking
{
    public enum ServerToClientMessageType : ushort
    {
        ConnectToGameSceneCommand = 1,
        CreateEntity = 2,
        DestroyEntity = 3,

        UpdateComponentsForEntity = 30,
        SynchronizeGlobalPosition = 31,
        SynchronizeRotation = 32,
    }
}
