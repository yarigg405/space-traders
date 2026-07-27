using Assets.Code.Common.Inventory;
using System;
using System.Collections.Generic;


namespace Assets.Code.UI.Screens.TradingMain
{
    public sealed class TradeCategoryBuildContext
    {
        public TradeCategoryRowView CategoryRowPrefab { get; }
        public TradeItemRowView ItemRowPrefab { get; }
        public IReadOnlyDictionary<string, List<ItemSO>> ItemsByCategory { get; }
        public Action OnHierarchyChanged { get; }
        public Action<ItemSO> OnItemSelected { get; }
        public bool ExpandByDefault { get; }

        public TradeCategoryBuildContext(
            TradeCategoryRowView categoryRowPrefab,
            TradeItemRowView itemRowPrefab,
            IReadOnlyDictionary<string, List<ItemSO>> itemsByCategory,
            Action onHierarchyChanged,
            Action<ItemSO> onItemSelected,
            bool expandByDefault)
        {
            CategoryRowPrefab = categoryRowPrefab;
            ItemRowPrefab = itemRowPrefab;
            ItemsByCategory = itemsByCategory;
            OnHierarchyChanged = onHierarchyChanged;
            OnItemSelected = onItemSelected;
            ExpandByDefault = expandByDefault;
        }
    }
}
