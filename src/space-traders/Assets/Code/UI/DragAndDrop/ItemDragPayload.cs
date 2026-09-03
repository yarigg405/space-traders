using Assets.Code.Common.Inventory;


namespace Assets.Code.UI.DragAndDrop
{
    public readonly struct ItemDragPayload
    {
        public readonly ItemSO Item;
        public readonly int Amount;
        public readonly int StationId;

        public ItemDragPayload(ItemSO item, int amount, int stationId)
        {
            Item = item;
            Amount = amount;
            StationId = stationId;
        }
    }
}
