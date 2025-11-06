namespace Assets.Code.Networking.MessageTypes
{
    public enum ServerToClientMessageType : ushort
    {
        ConnectToGameSceneCommand = 1,
        CreateEntity = 2,
        DestroyEntity=3,
    }
}
