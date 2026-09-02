using Assets.Code.Common.Inventory;


namespace Assets.Code.UI.Screens.StationsInventory
{
    public readonly struct AllAssetsItemModel
    {
        public readonly ItemSO Item;
        public readonly int Amount;

        public AllAssetsItemModel(ItemSO item, int amount)
        {
            Item = item;
            Amount = amount;
        }
    }
}
