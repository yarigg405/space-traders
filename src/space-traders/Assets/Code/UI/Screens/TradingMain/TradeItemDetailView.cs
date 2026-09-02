using Assets.Code.Common.Inventory;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;
using Yrr.UI.Elements;
using Yrr.Utils;


namespace Assets.Code.UI.Screens.TradingMain
{
    public sealed class TradeItemDetailView : MonoBehaviour
    {
        private const string LocalizationTable = "LocalizationTable";

        [Header("Header")]
        [SerializeField] private GameObject _emptyState;
        [SerializeField] private GameObject _content;
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _nameLabel;
        [SerializeField] private LocalizeableTmp _massTmp;
        [SerializeField] private LocalizeableTmp _volumeTmp;

        [Header("Buy orders")]
        [SerializeField] private Transform _buyOrdersRoot;
        [SerializeField] private GameObject _buyOrdersEmpty;
        [SerializeField] private GameObject _buyOrdersFilled;

        [Header("Sell orders")]
        [SerializeField] private RectTransform _sellOrdersRoot;
        [SerializeField] private GameObject _sellOrdersEmpty;
        [SerializeField] private GameObject _sellOrdersFilled;

        [Header("Prefabs")]
        [SerializeField] private TradeOrderRowView _orderRowPrefab;

        public event Action<ItemSO, TradeOrderInfo> BuyRequested;

        private LocalizedString _nameString;
        private ItemSO _item;

        private void Awake()
        {
            Clear();
        }

        public void Show(ItemSO item, IReadOnlyList<TradeOrderInfo> buyOrders, IReadOnlyList<TradeOrderInfo> sellOrders)
        {
            if (item == null)
            {
                Clear();
                return;
            }

            _item = item;

            _emptyState.SetActive(false);
            _content.SetActive(true);

            _icon.sprite = item.Icon;
            _icon.enabled = item.Icon;

            BindName(item.Id);
            _massTmp.BindText("massVal", item.Mass.ToString("N0"));
            _volumeTmp.BindText("volumeVal", item.Volume.ToString("N0"));

            FillOrders(_buyOrdersRoot, _buyOrdersEmpty, buyOrders, OnBuyOrderSelected);
            FillOrders(_sellOrdersRoot, _sellOrdersEmpty, sellOrders, OnSellOrderSelected);
        }

        public void Clear()
        {
            _item = null;

            _content.SetActive(false);
            _emptyState.SetActive(true);

            ClearOrders(_buyOrdersRoot, _buyOrdersEmpty);
            ClearOrders(_sellOrdersRoot, _sellOrdersEmpty);
        }

        private void OnBuyOrderSelected(TradeOrderInfo order)
        {
            OpenBuyOrderPopup(_item, order);
        }

        private void OnSellOrderSelected(TradeOrderInfo order)
        {
            if (_item != null)
                BuyRequested?.Invoke(_item, order);
        }

        private void OpenBuyOrderPopup(ItemSO item, TradeOrderInfo order)
        {
        }

        private void FillOrders(Transform root, GameObject emptyState,
            IReadOnlyList<TradeOrderInfo> orders, Action<TradeOrderInfo> onSelected)
        {
            root.ClearChildren();

            bool hasOrders = orders != null && orders.Count > 0;
            emptyState.SetActive(!hasOrders);

            if (!hasOrders)
                return;

            foreach (var order in orders)
            {
                var row = Instantiate(_orderRowPrefab, root);
                row.Bind(order, onSelected);
            }
        }

        private void ClearOrders(Transform root, GameObject emptyState)
        {
            root.ClearChildren();

            emptyState.SetActive(true);
        }

        private void BindName(string entryKey)
        {
            UnbindName();

            _nameString = new LocalizedString
            {
                TableReference = LocalizationTable,
                TableEntryReference = entryKey
            };

            _nameString.StringChanged += OnNameChanged;
            _nameString.RefreshString();
        }

        private void UnbindName()
        {
            if (_nameString == null)
                return;

            _nameString.StringChanged -= OnNameChanged;
            _nameString = null;
        }

        private void OnNameChanged(string value)
        {
            _nameLabel.text = value;
        }

        private void OnDestroy()
        {
            UnbindName();
        }
    }
}
