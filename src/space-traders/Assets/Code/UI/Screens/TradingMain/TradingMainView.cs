using Assets.Code.Common.Inventory;
using Assets.Code.Common.Inventory.Components;
using Assets.Code.Common.TradingSystem;
using Assets.Code.Networking.Data;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;
using Yrr.Utils;


namespace Assets.Code.UI.Screens.TradingMain
{
    public sealed class TradingMainView : UIScreenView
    {
        private const string LocalizationTable = "LocalizationTable";

        private static readonly TradeScope[] ScopeOrder = (TradeScope[])Enum.GetValues(typeof(TradeScope));

        [Header("Category tree")]
        [SerializeField] private RectTransform _categoriesRoot;
        [SerializeField] private TradeCategoryRowView _categoryRowPrefab;
        [SerializeField] private TradeItemRowView _itemRowPrefab;

        [Header("Filtering")]
        [SerializeField] private TMP_Dropdown _scopeDropdown;
        [SerializeField] private TMP_InputField _searchInput;

        [Header("Detail")]
        [SerializeField] private TradeItemDetailView _detailView;

        private readonly List<TradeCategoryRowView> _rootRows = new();
        private readonly Dictionary<string, ItemOrders> _ordersByItem = new();
        private readonly List<ScopeLabel> _scopeLabels = new();

        private IReadOnlyList<TradeItemCategory> _categories;
        private List<ItemSO> _items = new();
        private StationTradeData _tradeData;
        private bool _listenersWired;

        private TradeScope Scope
        {
            get
            {
                if (_scopeDropdown == null)
                    return TradeScope.CurrentStation;

                int index = Mathf.Clamp(_scopeDropdown.value, 0, ScopeOrder.Length - 1);
                return ScopeOrder[index];
            }
        }

        private string Search => _searchInput != null ? _searchInput.text : string.Empty;

        public void Setup(IEnumerable<TradeItemCategory> categories, IEnumerable<ItemSO> items)
        {
            _categories = new List<TradeItemCategory>(categories);

            _items = new List<ItemSO>();
            foreach (var item in items)
                if (item != null)
                    _items.Add(item);

            WireListeners();
            Rebuild();
        }

        public void SetTradeData(StationTradeData tradeData)
        {
            _tradeData = tradeData;
            Rebuild();
        }

        private void WireListeners()
        {
            if (_listenersWired)
                return;

            if (_scopeDropdown)
            {
                PopulateScopeOptions();
                _scopeDropdown.onValueChanged.AddListener(OnScopeChanged);
            }

            if (_searchInput)
                _searchInput.onValueChanged.AddListener(OnSearchChanged);

            _listenersWired = true;
        }

        private void PopulateScopeOptions()
        {
            DisposeScopeLabels();
            _scopeDropdown.ClearOptions();

            var options = new List<TMP_Dropdown.OptionData>(ScopeOrder.Length);
            foreach (var scope in ScopeOrder)
                options.Add(new TMP_Dropdown.OptionData(scope.ToString()));

            _scopeDropdown.options = options;
            _scopeDropdown.SetValueWithoutNotify(0);
            _scopeDropdown.RefreshShownValue();

            for (int i = 0; i < ScopeOrder.Length; i++)
                _scopeLabels.Add(new ScopeLabel(_scopeDropdown, i, ScopeOrder[i]));
        }

        private void DisposeScopeLabels()
        {
            foreach (var label in _scopeLabels)
                label.Dispose();

            _scopeLabels.Clear();
        }

        private void OnScopeChanged(int _) => Rebuild();

        private void OnSearchChanged(string _) => Rebuild();

        private void Rebuild()
        {
            if (_categories == null)
                return;

            BuildOrderLookup();

            _detailView?.Clear();
            ClearCategories();

            var itemsByCategory = GroupItemsByCategory(_items);
            var context = new TradeCategoryBuildContext(
                _categoryRowPrefab, _itemRowPrefab, itemsByCategory, RebuildLayout, OnItemSelected);

            foreach (var category in _categories)
            {
                var row = Instantiate(_categoryRowPrefab, _categoriesRoot);
                row.Bind(category, context);
                _rootRows.Add(row);
            }

            RebuildLayout();
        }

        private void OnItemSelected(ItemSO item)
        {
            if (item == null)
                return;

            if (_ordersByItem.TryGetValue(item.Id, out var orders))
                _detailView?.Show(item, orders.Buy, orders.Sell);
            else
                _detailView?.Show(item, Array.Empty<TradeOrderInfo>(), Array.Empty<TradeOrderInfo>());
        }

        private void BuildOrderLookup()
        {
            _ordersByItem.Clear();

            if (_tradeData.Stations == null)
                return;

            foreach (var station in _tradeData.Stations)
            {
                if (Scope == TradeScope.CurrentStation && station.StationId != _tradeData.CurrentStationId)
                    continue;

                AddOrders(station, station.SellOrders, isSell: true);
                AddOrders(station, station.BuyOrders, isSell: false);
            }
        }

        private void AddOrders(StationOrdersData station, List<TradeOrderData> orders, bool isSell)
        {
            if (orders == null)
                return;

            foreach (var order in orders)
            {
                if (string.IsNullOrEmpty(order.ItemId))
                    continue;

                if (!_ordersByItem.TryGetValue(order.ItemId, out var bucket))
                {
                    bucket = new ItemOrders();
                    _ordersByItem.Add(order.ItemId, bucket);
                }

                var info = new TradeOrderInfo(station.StationName, order.Price, order.Quantity, order.ExpiresAt);
                if (isSell)
                    bucket.Sell.Add(info);
                else
                    bucket.Buy.Add(info);
            }
        }

        private Dictionary<string, List<ItemSO>> GroupItemsByCategory(IEnumerable<ItemSO> items)
        {
            var map = new Dictionary<string, List<ItemSO>>();
            var search = Search;

            foreach (var item in items)
            {
                if (item == null)
                    continue;

                if (!_ordersByItem.ContainsKey(item.Id))
                    continue;

                if (!MatchesSearch(item, search))
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

        private static bool MatchesSearch(ItemSO item, string search)
        {
            if (string.IsNullOrWhiteSpace(search))
                return true;

            return item.Id != null &&
                   item.Id.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0;
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

        private void OnDestroy()
        {
            if (_scopeDropdown)
                _scopeDropdown.onValueChanged.RemoveListener(OnScopeChanged);

            if (_searchInput)
                _searchInput.onValueChanged.RemoveListener(OnSearchChanged);

            DisposeScopeLabels();
        }

        private sealed class ItemOrders
        {
            public readonly List<TradeOrderInfo> Sell = new();
            public readonly List<TradeOrderInfo> Buy = new();
        }

        private sealed class ScopeLabel
        {
            private readonly TMP_Dropdown _dropdown;
            private readonly int _index;
            private readonly LocalizedString _string;

            public ScopeLabel(TMP_Dropdown dropdown, int index, TradeScope scope)
            {
                _dropdown = dropdown;
                _index = index;

                _string = new LocalizedString
                {
                    TableReference = LocalizationTable,
                    TableEntryReference = $"TradeScope.{scope}"
                };

                _string.StringChanged += OnChanged;
                _string.RefreshString();
            }

            public void Dispose()
            {
                _string.StringChanged -= OnChanged;
            }

            private void OnChanged(string value)
            {
                if (_dropdown == null || string.IsNullOrEmpty(value) || _index >= _dropdown.options.Count)
                    return;

                _dropdown.options[_index].text = value;
                _dropdown.RefreshShownValue();
            }
        }
    }
}
