using System.Collections.Generic;


namespace Assets.Code.Networking.Data
{
    public struct StationTradeData
    {
        public int CurrentStationId;
        public int CurrentStarSystemId;
        public double CurrentPositionX;
        public double CurrentPositionY;
        public List<StationOrdersData> Stations;
    }

    public struct StationOrdersData
    {
        public int StationId;
        public string StationName;
        public double PositionX;
        public double PositionY;
        public int StarSystemId;
        public string StarSystemName;
        public List<TradeOrderData> BuyOrders;
        public List<TradeOrderData> SellOrders;
    }

    public struct TradeOrderData
    {
        public long Id;
        public string ItemId;
        public long Price;
        public int Quantity;
        public long ExpiresAt;
    }
}
