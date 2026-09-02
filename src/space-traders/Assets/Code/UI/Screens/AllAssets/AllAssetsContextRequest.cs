using Assets.Code.Common.Inventory;
using UnityEngine;


namespace Assets.Code.UI.Screens.StationsInventory
{
    public readonly struct AllAssetsContextRequest
    {
        public readonly ItemSO Item;
        public readonly int Amount;
        public readonly int StationId;
        public readonly string StationName;
        public readonly string SystemName;
        public readonly Vector2 Position;

        public AllAssetsContextRequest(ItemSO item, int amount, int stationId,
            string stationName, string systemName, Vector2 position)
        {
            Item = item;
            Amount = amount;
            StationId = stationId;
            StationName = stationName;
            SystemName = systemName;
            Position = position;
        }
    }
}
