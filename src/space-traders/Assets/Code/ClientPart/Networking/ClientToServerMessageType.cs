namespace Assets.Code.ClientPart.Networking
{
    public enum ClientToServerMessageType : ushort
    {
        RequestGetCharacters = 1,
        RequestCreateCharacter = 2,

        RequestEnterTheGame = 3,
        RequestForSceneEntities = 4,
        RequestForStationSceneData = 5,
        RequestForUndock = 6,
        RequestForDock = 7,
        RequestForMoney = 8,
        RequestItemOrders = 9,
        RequestBuyItem = 10,

        SendTargetRotation = 31,
        SendSpeedModifier = 32,
        SendKeepDistance = 33,
        SendSetOrbit = 34,
        SendSetWarpTo = 35,
    }
}
