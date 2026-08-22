using Assets.Code.ClientPart.Networking;
using Assets.Code.Common.Extensions;
using Assets.Code.Common.Inventory;
using Assets.Code.Infrastructure.Loading;
using Assets.Code.UI.Infrastructure.Interfaces;
using Assets.Code.UI.Screens.MessageBox;
using Assets.Code.UI.Screens.TradingMain;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;


namespace Assets.Code.UI.Screens.BuySellPopups
{
    public sealed class BuyItemPresenter : IPresenter<BuyItemView>
    {
        private const string LocalizationTable = "LocalizationTable";

        private readonly IUIManager _uiManager;
        private readonly ClientMessenger _messenger;
        private readonly StationSceneDataHolder _stationSceneDataHolder;

        private BuyItemView _view;
        private LocalizedString _nameString;
        private ItemSO _item;
        private TradeOrderInfo _order;
        private Action _onPurchased;
        private int _quantity;
        private CancellationTokenSource _cts;

        public BuyItemPresenter(IUIManager uiManager, ClientMessenger messenger,
            StationSceneDataHolder stationSceneDataHolder)
        {
            _uiManager = uiManager;
            _messenger = messenger;
            _stationSceneDataHolder = stationSceneDataHolder;
        }

        void IPresenter<BuyItemView>.Show(BuyItemView view, object args)
        {
            if (args is not BuyItemArgs data || data.Item == null)
            {
                Debug.LogError("BuyItemView needs BuyItemArgs with an item");
                return;
            }

            _view = view;
            _item = data.Item;
            _order = data.Order;
            _onPurchased = data.OnPurchased;
            _cts = new();

            if (view.ItemIcon)
            {
                view.ItemIcon.sprite = _item.Icon;
                view.ItemIcon.enabled = _item.Icon;
            }

            BindName(_item.Id);

            view.LocationNameTmp.text = $"{_order.SystemName}\n{_order.StationName}";
            var atOrderStation = _stationSceneDataHolder.Current.StationId == _order.StationId;
            view.LocationNameTmp.color = atOrderStation ? Color.green : Color.yellow;

            view.PriceTmp.text = _order.Price.ToMoney();
            view.MaxQuantityTmp.text = _order.Quantity.ToString("N0");

            view.QuantityIF.contentType = TMP_InputField.ContentType.IntegerNumber;
            view.QuantityIF.onValueChanged.AddListener(OnQuantityChanged);
            view.IncreaseQuantityBtn.onClick.AddListener(OnIncrease);
            view.DecreaseQuantityBtn.onClick.AddListener(OnDecrease);
            view.ConfirmPurchaseBtn.onClick.AddListener(OnConfirm);
            view.CancelPurchaseBtn.onClick.AddListener(Close);
            view.CloseButton.onClick.AddListener(Close);

            view.ConfirmPurchaseBtn.interactable = true;
            SetQuantity(1);
        }

        void IPresenter<BuyItemView>.Hide(BuyItemView view)
        {
            view.QuantityIF.onValueChanged.RemoveListener(OnQuantityChanged);
            view.IncreaseQuantityBtn.onClick.RemoveListener(OnIncrease);
            view.DecreaseQuantityBtn.onClick.RemoveListener(OnDecrease);
            view.ConfirmPurchaseBtn.onClick.RemoveListener(OnConfirm);
            view.CancelPurchaseBtn.onClick.RemoveListener(Close);
            view.CloseButton.onClick.RemoveListener(Close);

            UnbindName();

            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
            _onPurchased = null;
            _view = null;
        }

        private void OnQuantityChanged(string value)
        {
            var quantity = int.TryParse(value, out var parsed) ? parsed : 1;
            SetQuantity(quantity);
        }

        private void OnIncrease() => SetQuantity(_quantity + 1);

        private void OnDecrease() => SetQuantity(_quantity - 1);

        private void SetQuantity(int value)
        {
            var max = Mathf.Max(1, _order.Quantity);
            _quantity = Mathf.Clamp(value, 1, max);

            _view.QuantityIF.SetTextWithoutNotify(_quantity.ToString());
            UpdateTotals();
        }

        private void UpdateTotals()
        {
            _view.TotalPriceTmp.text = (_order.Price * _quantity).ToMoney();
            _view.TotalVolumeTmp.BindText("volumeVal", (_item.Volume * _quantity).ToString("N0"));
            _view.TotalMassTmp.BindText("massVal", (_item.Mass * _quantity).ToString("N0"));
        }

        private void OnConfirm()
        {
            ConfirmAsync(_cts.Token).Forget();
        }

        private async UniTask ConfirmAsync(CancellationToken ct)
        {
            try
            {
                _view.ConfirmPurchaseBtn.interactable = false;

                await _messenger.RequestForBuyItem(_order.OrderId, _quantity, ct);
                ct.ThrowIfCancellationRequested();

                _onPurchased?.Invoke();
                _uiManager.CloseModal<BuyItemPopup>();
            }
            catch (OperationCanceledException)
            {
            }
            catch (RequestFailedException ex)
            {
                if (_view != null)
                    _view.ConfirmPurchaseBtn.interactable = true;

                _uiManager.OpenModal<MessageBoxPopup>(ex.Message);
            }
            catch (Exception ex)
            {
                if (_view != null)
                    _view.ConfirmPurchaseBtn.interactable = true;

                Debug.LogError($"Buy request failed: {ex}");
            }
        }

        private void Close()
        {
            _uiManager.CloseModal<BuyItemPopup>();
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
            if (_view != null && _view.ItemNameTmp)
                _view.ItemNameTmp.text = value;
        }
    }
}
