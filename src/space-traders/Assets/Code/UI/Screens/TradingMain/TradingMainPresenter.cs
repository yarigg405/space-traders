using Assets.Code.ClientPart.Networking;
using Assets.Code.Common.Inventory;
using Assets.Code.Common.StaticData;
using Assets.Code.Common.TradingSystem;
using Assets.Code.UI.Infrastructure.Interfaces;
using Assets.Code.UI.Screens.BuySellPopups;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;


namespace Assets.Code.UI.Screens.TradingMain
{
    public sealed class TradingMainPresenter : IPresenter<TradingMainView>
    {
        private readonly IUIManager _uiManager;
        private readonly TradeItemsCategoryConfig _categoriesConfig;
        private readonly IItemsCatalog _itemsCatalog;
        private readonly ClientMessenger _messenger;

        private TradingMainView _view;
        private CancellationTokenSource _itemRequestCts;

        public TradingMainPresenter(IUIManager uiManager, TradeItemsCategoryConfig categoriesConfig,
            IItemsCatalog itemsCatalog, ClientMessenger messenger)
        {
            _uiManager = uiManager;
            _categoriesConfig = categoriesConfig;
            _itemsCatalog = itemsCatalog;
            _messenger = messenger;
        }

        void IPresenter<TradingMainView>.Show(TradingMainView view, object args)
        {
            _view = view;

            view.Setup(_categoriesConfig.GetAllCategories(), _itemsCatalog.GetAllItems());
            view.CloseButton.onClick.AddListener(ClickOnClose);
            view.BuyRequested += OnBuyRequested;
            view.ItemSelected += OnItemSelected;
        }

        void IPresenter<TradingMainView>.Hide(TradingMainView view)
        {
            view.CloseButton.onClick.RemoveListener(ClickOnClose);
            view.BuyRequested -= OnBuyRequested;
            view.ItemSelected -= OnItemSelected;

            _itemRequestCts?.Cancel();
            _itemRequestCts?.Dispose();
            _itemRequestCts = null;
            _view = null;
        }

        private void OnItemSelected(ItemSO item) => RequestItemOrders(item);

        private void RequestItemOrders(ItemSO item)
        {
            _itemRequestCts?.Cancel();
            _itemRequestCts?.Dispose();
            _itemRequestCts = new();

            LoadItemOrdersAsync(item, _itemRequestCts.Token).Forget();
        }

        private async UniTask LoadItemOrdersAsync(ItemSO item, CancellationToken ct)
        {
            try
            {
                var data = await _messenger.RequestForItemOrders(item.Id, ct);
                ct.ThrowIfCancellationRequested();

                _view?.SetItemOrders(item, data);
            }
            catch (System.OperationCanceledException)
            {
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Failed to load item orders: {ex}");
            }
        }

        private void OnBuyRequested(ItemSO item, TradeOrderInfo order)
        {
            _uiManager.OpenModal<BuyItemPopup>(new BuyItemArgs(item, order, () => RequestItemOrders(item)));
        }

        private void ClickOnClose()
        {
            _uiManager.CloseModal<TradingMainPopup>();
        }
    }
}
