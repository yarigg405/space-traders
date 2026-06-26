using Assets.Code.Common.Inventory;

namespace Assets.Code.Common.StaticData
{
    public interface IItemsCatalog
    {
        ItemSO GetItem(string id);
    }
}