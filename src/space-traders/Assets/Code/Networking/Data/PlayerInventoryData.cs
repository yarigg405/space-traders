using System.Collections.Generic;


namespace Assets.Code.Networking.Data
{
    public struct PlayerInventoryData
    {
        public List<StationInventoryData> Stations;
    }

    public struct StationInventoryData
    {
        public int StationId;
        public string StationName;
        public string SystemName;
        public List<InventoryItemData> Items;
    }

    public struct InventoryItemData
    {
        public string ItemId;
        public int Amount;
    }
}
