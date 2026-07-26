using System.Collections.Generic;


namespace Assets.Code.Networking.Data
{
    public struct StationTradeData
    {
        public int CurrentStationId;
        public List<StationOrdersData> Stations;
    }

    public struct StationOrdersData
    {
        public int StationId;
        public string StationName;
        public List<TradeOrderData> BuyOrders;
        public List<TradeOrderData> SellOrders;
    }

    public struct TradeOrderData
    {
        public string ItemId;
        public long Price;
        public int Quantity;
        public long ExpiresAt;
    }
}
