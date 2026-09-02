using Assets.Code.Common.Extensions;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Assets.Code.UI.Screens.TradingMain
{
    public sealed class TradeOrderRowView : MonoBehaviour
    {
        [SerializeField] private Button _selectButton;
        [SerializeField] private TextMeshProUGUI _stationLabel;
        [SerializeField] private TextMeshProUGUI _distanceLabel;
        [SerializeField] private TextMeshProUGUI _priceLabel;
        [SerializeField] private TextMeshProUGUI _quantityLabel;
        [SerializeField] private TextMeshProUGUI _expiresLabel;

        private TradeOrderInfo _order;
        private Action<TradeOrderInfo> _onSelected;

        public void Bind(TradeOrderInfo order, Action<TradeOrderInfo> onSelected)
        {
            _order = order;
            _onSelected = onSelected;

            var color = order.IsAtPlayerStation ? Color.green : Color.white;

            _stationLabel.text = order.StationName;
            _stationLabel.color = color;

            _distanceLabel.text = order.Distance;
            _distanceLabel.color = color;

            _priceLabel.text = order.Price.ToMoney();
            _priceLabel.color = color;

            _quantityLabel.text = order.Quantity.ToString("N0");
            _quantityLabel.color = color;

            _expiresLabel.text = FormatExpires(order.ExpiresAt);
            _expiresLabel.color = color;

            _selectButton.onClick.RemoveListener(OnSelectClicked);
            _selectButton.onClick.AddListener(OnSelectClicked);
        }

        private void OnSelectClicked()
        {
            _onSelected?.Invoke(_order);
        }

        private static string FormatExpires(long unixSeconds)
        {
            if (unixSeconds <= 0)
                return string.Empty;

            return DateTimeOffset.FromUnixTimeSeconds(unixSeconds).LocalDateTime.ToString("dd.MM.yyyy HH:mm");
        }

        private void OnDestroy()
        {
            _selectButton.onClick.RemoveListener(OnSelectClicked);
        }
    }

    public readonly struct TradeOrderInfo
    {
        public readonly long OrderId;
        public readonly int StationId;
        public readonly string StationName;
        public readonly string SystemName;
        public readonly string Distance;
        public readonly long Price;
        public readonly int Quantity;
        public readonly long ExpiresAt;
        public readonly bool IsAtPlayerStation;

        public TradeOrderInfo(long orderId, int stationId, string stationName, string systemName,
            string distance, long price, int quantity, long expiresAt, bool isAtPlayerStation)
        {
            OrderId = orderId;
            StationId = stationId;
            StationName = stationName;
            SystemName = systemName;
            Distance = distance;
            Price = price;
            Quantity = quantity;
            ExpiresAt = expiresAt;
            IsAtPlayerStation = isAtPlayerStation;
        }
    }
}
