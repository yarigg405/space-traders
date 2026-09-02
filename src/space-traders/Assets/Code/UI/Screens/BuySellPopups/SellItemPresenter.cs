using Assets.Code.ClientPart.Networking;
using Assets.Code.Common.Extensions;
using Assets.Code.Common.Inventory;
using Assets.Code.Infrastructure.Loading;
using Assets.Code.Networking.Data;
using Assets.Code.UI.Infrastructure.Interfaces;
using Assets.Code.UI.Screens.MessageBox;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;


namespace Assets.Code.UI.Screens.BuySellPopups
{
    public sealed class SellItemPresenter : IPresenter<SellItemView>
    {
        private const string LocalizationTable = "LocalizationTable";

        private readonly IUIManager _uiManager;
        private readonly ClientMessenger _messenger;
        private readonly StationSceneDataHolder _stationSceneDataHolder;

        private SellItemView _view;
        private LocalizedString _nameString;
        private ItemSO _item;
        private int _stationId;
        private int _available;
        private Action _onSold;

        private bool _hasOrder;
        private BestBuyOrderData _order;
        private int _maxQuantity;
        private int _quantity;

        private CancellationTokenSource _cts;

        public SellItemPresenter(IUIManager uiManager, ClientMessenger messenger,
            StationSceneDataHolder stationSceneDataHolder)
        {
            _uiManager = uiManager;
            _messenger = messenger;
            _stationSceneDataHolder = stationSceneDataHolder;
        }

        void IPresenter<SellItemView>.Show(SellItemView view, object args)
        {
            if (args is not SellItemArgs data || data.Item == null)
            {
                Debug.LogError("SellItemView needs SellItemArgs with an item");
                return;
            }

            _view = view;
            _item = data.Item;
            _stationId = data.StationId;
            _available = data.Available;
            _onSold = data.OnSold;
            _hasOrder = false;
            _cts = new();

            if (view.ItemIcon)
            {
                view.ItemIcon.sprite = _item.Icon;
                view.ItemIcon.enabled = _item.Icon;
            }

            BindName(_item.Id);

            view.LocationNameTmp.text = $"{data.SystemName}\n{data.StationName}";
            var atStation = _stationSceneDataHolder.Current.StationId == _stationId;
            view.LocationNameTmp.color = atStation ? Color.green : Color.yellow;

            view.QuantityIF.contentType = TMP_InputField.ContentType.IntegerNumber;
            view.QuantityIF.onValueChanged.AddListener(OnQuantityChanged);
            view.IncreaseQuantityBtn.onClick.AddListener(OnIncrease);
            view.DecreaseQuantityBtn.onClick.AddListener(OnDecrease);
            view.ConfirmSellBtn.onClick.AddListener(OnConfirm);
            view.CancelSellBtn.onClick.AddListener(Close);
            view.CloseButton.onClick.AddListener(Close);

            view.ConfirmSellBtn.interactable = false;
            ShowEmpty();

            LoadOrderAsync(_cts.Token).Forget();
        }

        void IPresenter<SellItemView>.Hide(SellItemView view)
        {
            view.QuantityIF.onValueChanged.RemoveListener(OnQuantityChanged);
            view.IncreaseQuantityBtn.onClick.RemoveListener(OnIncrease);
            view.DecreaseQuantityBtn.onClick.RemoveListener(OnDecrease);
            view.ConfirmSellBtn.onClick.RemoveListener(OnConfirm);
            view.CancelSellBtn.onClick.RemoveListener(Close);
            view.CloseButton.onClick.RemoveListener(Close);

            UnbindName();

            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
            _onSold = null;
            _view = null;
        }

        private async UniTask LoadOrderAsync(CancellationToken ct)
        {
            try
            {
                var order = await _messenger.RequestForBestBuyOrder(_item.Id, _stationId, ct);
                ct.ThrowIfCancellationRequested();

                if (order.Found)
                {
                    _order = order;
                    _hasOrder = true;
                    _maxQuantity = Mathf.Min(order.Quantity, _available);

                    _view.PriceTmp.text = order.Price.ToMoney();
                    _view.MaxQuantityTmp.text = _maxQuantity.ToString("N0");
                    _view.ConfirmSellBtn.interactable = _maxQuantity > 0;

                    SetQuantity(1);
                }
                else
                {
                    Debug.LogError("No orders found");
                    ShowEmpty();
                }
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to load sell order: {ex}");

                if (_view != null)
                    ShowEmpty();
            }
        }

        private void ShowEmpty()
        {
            _hasOrder = false;
            _maxQuantity = 0;
            _quantity = 0;

            _view.PriceTmp.text = 0L.ToMoney();
            _view.MaxQuantityTmp.text = "0";
            _view.QuantityIF.SetTextWithoutNotify("0");
            _view.ConfirmSellBtn.interactable = false;

            UpdateTotals();
        }

        private void OnQuantityChanged(string value)
        {
            var quantity = int.TryParse(value, out var parsed) ? parsed : 0;
            SetQuantity(quantity);
        }

        private void OnIncrease() => SetQuantity(_quantity + 1);

        private void OnDecrease() => SetQuantity(_quantity - 1);

        private void SetQuantity(int value)
        {
            if (!_hasOrder || _maxQuantity < 1)
            {
                _quantity = 0;
                _view.QuantityIF.SetTextWithoutNotify("0");
                UpdateTotals();
                return;
            }

            _quantity = Mathf.Clamp(value, 1, _maxQuantity);
            _view.QuantityIF.SetTextWithoutNotify(_quantity.ToString());
            UpdateTotals();
        }

        private void UpdateTotals()
        {
            var price = _hasOrder ? _order.Price : 0L;
            _view.TotalPriceTmp.text = (price * _quantity).ToMoney();
            _view.TotalVolumeTmp.BindText("volumeVal", (_item.Volume * _quantity).ToString("N0"));
            _view.TotalMassTmp.BindText("massVal", (_item.Mass * _quantity).ToString("N0"));
        }

        private void OnConfirm()
        {
            ConfirmAsync(_cts.Token).Forget();
        }

        private async UniTask ConfirmAsync(CancellationToken ct)
        {
            if (!_hasOrder || _quantity < 1)
                return;

            try
            {
                _view.ConfirmSellBtn.interactable = false;

                await _messenger.RequestForSellItem(_order.OrderId, _quantity, ct);
                ct.ThrowIfCancellationRequested();

                _onSold?.Invoke();
                _uiManager.CloseModal<SellItemPopup>();
            }
            catch (OperationCanceledException)
            {
            }
            catch (RequestFailedException ex)
            {
                if (_view != null)
                    _view.ConfirmSellBtn.interactable = true;

                _uiManager.OpenModal<MessageBoxPopup>(ex.Message);
            }
            catch (Exception ex)
            {
                if (_view != null)
                    _view.ConfirmSellBtn.interactable = true;

                Debug.LogError($"Sell request failed: {ex}");
            }
        }

        private void Close()
        {
            _uiManager.CloseModal<SellItemPopup>();
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
