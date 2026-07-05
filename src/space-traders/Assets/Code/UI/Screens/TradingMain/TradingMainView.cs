using Assets.Code.Common.Inventory;
using Assets.Code.Common.Inventory.Components;
using Assets.Code.Common.TradingSystem;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yrr.Utils;


namespace Assets.Code.UI.Screens.TradingMain
{
    public sealed class TradingMainView : UIScreenView
    {
        [SerializeField] private RectTransform _categoriesRoot;
        [SerializeField] private TradeCategoryRowView _categoryRowPrefab;
        [SerializeField] private TradeItemRowView _itemRowPrefab;

        private readonly List<TradeCategoryRowView> _rootRows = new();

        public void SetupCategories(IEnumerable<TradeItemCategory> categories, IEnumerable<ItemSO> items)
        {
            ClearCategories();

            var itemsByCategory = GroupItemsByCategory(items);
            var context = new TradeCategoryBuildContext(_categoryRowPrefab, _itemRowPrefab, itemsByCategory, RebuildLayout);

            foreach (var category in categories)
            {
                var row = Instantiate(_categoryRowPrefab, _categoriesRoot);
                row.Bind(category, context);
                _rootRows.Add(row);
            }

            RebuildLayout();
        }

        private static Dictionary<string, List<ItemSO>> GroupItemsByCategory(IEnumerable<ItemSO> items)
        {
            var map = new Dictionary<string, List<ItemSO>>();

            foreach (var item in items)
            {
                if (item == null)
                    continue;

                if (!item.Components.TryGetComponent<TradeItemComponent>(out var tradeItem))
                    continue;

                if (string.IsNullOrEmpty(tradeItem.CategoryId))
                    continue;

                if (!map.TryGetValue(tradeItem.CategoryId, out var list))
                {
                    list = new List<ItemSO>();
                    map.Add(tradeItem.CategoryId, list);
                }

                list.Add(item);
            }

            return map;
        }

        private void RebuildLayout()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(_categoriesRoot);
        }

        private void ClearCategories()
        {
            _rootRows.Clear();
            _categoriesRoot.ClearChildren();
        }
    }
}
