using Assets.Code.ClientPart.Gameplay.Features.Navigation;
using Assets.Code.Common;
using Assets.Code.Common.Inventory;
using Assets.Code.Common.Inventory.Components;
using Assets.Code.Common.TradingSystem;
using Assets.Code.Networking.Data;
using System;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
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
        [SerializeField] private Button _scopeButton;
        [SerializeField] private TextMeshProUGUI _scopeLabel;
        [SerializeField] private TMP_InputField _searchInput;

        [Header("Detail")]
        [SerializeField] private TradeItemDetailView _detailView;

        private readonly List<TradeCategoryRowView> _rootRows = new();
        private readonly Dictionary<string, ItemOrders> _ordersByItem = new();
        private readonly Dictionary<string, string> _localizedNames = new();
        private readonly Dictionary<int, string> _distanceByStation = new();

        private IReadOnlyList<TradeItemCategory> _categories;
        private List<ItemSO> _items = new();
        private StationTradeData _tradeData;
        private ItemSO _selectedItem;
        private LocalizedString _scopeLabelString;
        private int _scopeIndex;
        private bool _listenersWired;

        private TradeScope Scope => ScopeOrder[Mathf.Clamp(_scopeIndex, 0, ScopeOrder.Length - 1)];

        private string Search => _searchInput != null ? _searchInput.text : string.Empty;

        public event Action<ItemSO, TradeOrderInfo> BuyRequested
        {
            add { if (_detailView) _detailView.BuyRequested += value; }
            remove { if (_detailView) _detailView.BuyRequested -= value; }
        }

        public event Action<ItemSO> ItemSelected;

        public void Setup(IEnumerable<TradeItemCategory> categories, IEnumerable<ItemSO> items)
        {
            _categories = new List<TradeItemCategory>(categories);

            _items = new List<ItemSO>();
            foreach (var item in items)
                if (item != null)
                    _items.Add(item);

            _selectedItem = null;
            _tradeData = default;
            _ordersByItem.Clear();
            _distanceByStation.Clear();
            _scopeIndex = Mathf.Max(0, Array.IndexOf(ScopeOrder, TradeScope.AllSystems));

            WireListeners();
            UpdateScopeLabel();
            RefreshLocalizedNames();
            RebuildTree();
            _detailView?.Clear();
        }

        public void SetItemOrders(ItemSO item, StationTradeData data)
        {
            if (item != _selectedItem)
                return;

            _tradeData = data;
            BuildDistanceCache();
            BuildOrderLookup();
            ShowItem(item);
        }

        private void WireListeners()
        {
            if (_listenersWired)
                return;

            if (_scopeButton)
                _scopeButton.onClick.AddListener(OnScopeButtonClicked);

            if (_searchInput)
                _searchInput.onValueChanged.AddListener(OnSearchChanged);

            LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;

            _listenersWired = true;
        }

        private void RefreshLocalizedNames()
        {
            _localizedNames.Clear();

            foreach (var item in _items)
            {
                if (item == null || string.IsNullOrEmpty(item.Id))
                    continue;

                var localized = new LocalizedString
                {
                    TableReference = LocalizationTable,
                    TableEntryReference = item.Id
                };

                _localizedNames[item.Id] = localized.GetLocalizedString();
            }
        }

        private void OnLocaleChanged(Locale locale)
        {
            RefreshLocalizedNames();
            RebuildTree();
        }

        private void OnScopeButtonClicked()
        {
            _scopeIndex = (_scopeIndex + 1) % ScopeOrder.Length;
            UpdateScopeLabel();
            BuildOrderLookup();
            RefreshSelectedItem();
        }

        private void UpdateScopeLabel()
        {
            if (_scopeLabel == null)
                return;

            UnbindScopeLabel();

            _scopeLabelString = new LocalizedString
            {
                TableReference = LocalizationTable,
                TableEntryReference = $"TradeScope.{ScopeOrder[_scopeIndex]}"
            };

            _scopeLabelString.StringChanged += OnScopeLabelChanged;
            _scopeLabelString.RefreshString();
        }

        private void UnbindScopeLabel()
        {
            if (_scopeLabelString == null)
                return;

            _scopeLabelString.StringChanged -= OnScopeLabelChanged;
            _scopeLabelString = null;
        }

        private void OnScopeLabelChanged(string value)
        {
            if (_scopeLabel != null && !string.IsNullOrEmpty(value))
                _scopeLabel.text = value;
        }

        private void OnSearchChanged(string _) => RebuildTree();

        private void RebuildTree()
        {
            if (_categories == null)
                return;

            ClearCategories();

            var itemsByCategory = GroupItemsByCategory(_items);
            var expandByDefault = !string.IsNullOrWhiteSpace(Search);
            var context = new TradeCategoryBuildContext(
                _categoryRowPrefab, _itemRowPrefab, itemsByCategory, RebuildLayout, OnItemSelected, expandByDefault);

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

            _selectedItem = item;
            _tradeData = default;
            _ordersByItem.Clear();
            _distanceByStation.Clear();

            _detailView?.Show(item, Array.Empty<TradeOrderInfo>(), Array.Empty<TradeOrderInfo>());
            ItemSelected?.Invoke(item);
        }

        private void RefreshSelectedItem()
        {
            if (_selectedItem == null)
            {
                _detailView?.Clear();
                return;
            }

            ShowItem(_selectedItem);
        }

        private void ShowItem(ItemSO item)
        {
            if (_ordersByItem.TryGetValue(item.Id, out var orders))
                _detailView?.Show(item, orders.Buy, orders.Sell);
            else
                _detailView?.Show(item, Array.Empty<TradeOrderInfo>(), Array.Empty<TradeOrderInfo>());
        }

        private void BuildDistanceCache()
        {
            _distanceByStation.Clear();

            if (_tradeData.Stations == null)
                return;

            double2 from = new(_tradeData.CurrentPositionX, _tradeData.CurrentPositionY);

            foreach (var station in _tradeData.Stations)
            {
                if (station.StarSystemId == _tradeData.CurrentStarSystemId)
                {
                    double2 to = new(station.PositionX, station.PositionY);
                    double distance = math.distance(from, to) * GameConstants.DISTANCE_REAL_TO_UI;
                    _distanceByStation[station.StationId] = DistanceFormat.Format(distance);
                }
                else
                {
                    _distanceByStation[station.StationId] = station.StarSystemName;
                }
            }
        }

        private void BuildOrderLookup()
        {
            _ordersByItem.Clear();

            if (_tradeData.Stations == null)
                return;

            foreach (var station in _tradeData.Stations)
            {
                if (!IsInScope(station, _tradeData.CurrentStarSystemId))
                    continue;

                AddOrders(station, station.SellOrders, isSell: true);
                AddOrders(station, station.BuyOrders, isSell: false);
            }
        }

        private bool IsInScope(StationOrdersData station, int currentSystemId)
        {
            return Scope switch
            {
                TradeScope.CurrentStation => station.StationId == _tradeData.CurrentStationId,
                TradeScope.StarSystem => station.StarSystemId == currentSystemId,
                _ => true,
            };
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

                var distance = _distanceByStation.TryGetValue(station.StationId, out var d) ? d : string.Empty;
                var isAtPlayerStation = station.StationId == _tradeData.CurrentStationId;
                var info = new TradeOrderInfo(order.Id, station.StationId, station.StationName,
                    station.StarSystemName, distance, order.Price, order.Quantity, order.ExpiresAt, isAtPlayerStation);
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

        private bool MatchesSearch(ItemSO item, string search)
        {
            if (string.IsNullOrWhiteSpace(search))
                return true;

            var name = GetLocalizedName(item);
            return name != null &&
                   name.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private string GetLocalizedName(ItemSO item)
        {
            if (item == null || item.Id == null)
                return null;

            return _localizedNames.TryGetValue(item.Id, out var name) && !string.IsNullOrEmpty(name)
                ? name
                : item.Id;
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
            if (_scopeButton)
                _scopeButton.onClick.RemoveListener(OnScopeButtonClicked);

            if (_searchInput)
                _searchInput.onValueChanged.RemoveListener(OnSearchChanged);

            LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;

            UnbindScopeLabel();
        }

        private sealed class ItemOrders
        {
            public readonly List<TradeOrderInfo> Sell = new();
            public readonly List<TradeOrderInfo> Buy = new();
        }
    }
}
