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
        [SerializeField] private TextMeshProUGUI _priceLabel;
        [SerializeField] private TextMeshProUGUI _quantityLabel;
        [SerializeField] private TextMeshProUGUI _expiresLabel;

        private TradeOrderInfo _order;
        private Action<TradeOrderInfo> _onSelected;

        public void Bind(TradeOrderInfo order, Action<TradeOrderInfo> onSelected)
        {
            _order = order;
            _onSelected = onSelected;

            if (_stationLabel)
                _stationLabel.text = order.StationName;

            if (_priceLabel)
                _priceLabel.text = order.Price.ToString("N0");

            if (_quantityLabel)
                _quantityLabel.text = order.Quantity.ToString("N0");

            if (_expiresLabel)
                _expiresLabel.text = FormatExpires(order.ExpiresAt);

            if (_selectButton)
            {
                _selectButton.onClick.RemoveListener(OnSelectClicked);
                _selectButton.onClick.AddListener(OnSelectClicked);
            }
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
            if (_selectButton)
                _selectButton.onClick.RemoveListener(OnSelectClicked);
        }
    }

    public readonly struct TradeOrderInfo
    {
        public readonly string StationName;
        public readonly long Price;
        public readonly int Quantity;
        public readonly long ExpiresAt;

        public TradeOrderInfo(string stationName, long price, int quantity, long expiresAt)
        {
            StationName = stationName;
            Price = price;
            Quantity = quantity;
            ExpiresAt = expiresAt;
        }
    }
}
