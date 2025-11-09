namespace Assets.Code.ServerPart.Networking
{
    public enum ServerToClientMessageType : ushort
    {
        ConnectToGameSceneCommand = 1,
        CreateEntity = 2,
        DestroyEntity = 3,

        UpdateGlobalPosition = 30,
    }
}
