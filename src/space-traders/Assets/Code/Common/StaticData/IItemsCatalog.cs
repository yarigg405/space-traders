using Assets.Code.Common.Inventory;
using System.Collections.Generic;


namespace Assets.Code.Common.StaticData
{
    public interface IItemsCatalog
    {
        ItemSO GetItem(string id);
        IEnumerable<ItemSO> GetAllItems();
    }
}