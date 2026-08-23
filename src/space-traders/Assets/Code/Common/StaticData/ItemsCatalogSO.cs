using Assets.Code.Common.Inventory;
using System.Collections.Generic;
using UnityEngine;
using Yrr.Utils;


namespace Assets.Code.Common.StaticData
{
    [CreateAssetMenu(fileName = "ItemsCatalog", menuName = "ScriptableObjects/ItemsCatalogSO", order = 51)]
    public sealed class ItemsCatalogSO : ScriptableObject, IItemsCatalog
    {
        [ReadOnly]
        [SerializeField] private Dictionary<string, ItemSO> Items = new();

        internal void SetItems(IEnumerable<ItemSO> items)
        {
            Items.Clear();

            foreach (var item in items)
            {
                if (item == null)
                    continue;

                if (string.IsNullOrEmpty(item.Id))
                {
                    Debug.LogError($"ItemSO '{item.name}' has an empty Id and was skipped.");
                    continue;
                }

                if (Items.ContainsKey(item.Id))
                {
                    Debug.LogError($"Duplicate item Id '{item.Id}' found on '{item.name}' and was skipped.");
                    continue;
                }

                Items.Add(item.Id, item);
            }
        }

        ItemSO IItemsCatalog.GetItem(string id)
        {
            return Items[id];
        }

        IEnumerable<ItemSO> IItemsCatalog.GetAllItems()
        {
            return Items.Values;
        }
    }
}