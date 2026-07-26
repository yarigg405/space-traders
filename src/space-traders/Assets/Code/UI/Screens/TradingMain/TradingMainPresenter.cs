using Assets.Code.ClientPart.Networking;
using Assets.Code.Common.StaticData;
using Assets.Code.Common.TradingSystem;
using Assets.Code.Infrastructure.Loading;
using Assets.Code.UI.Infrastructure.Interfaces;
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
        private readonly StationSceneDataHolder _stationSceneDataHolder;

        private CancellationTokenSource _cts;

        public TradingMainPresenter(IUIManager uiManager, TradeItemsCategoryConfig categoriesConfig,
            IItemsCatalog itemsCatalog, ClientMessenger messenger, StationSceneDataHolder stationSceneDataHolder)
        {
            _uiManager = uiManager;
            _categoriesConfig = categoriesConfig;
            _itemsCatalog = itemsCatalog;
            _messenger = messenger;
            _stationSceneDataHolder = stationSceneDataHolder;
        }

        void IPresenter<TradingMainView>.Show(TradingMainView view, object args)
        {
            view.Setup(_categoriesConfig.GetAllCategories(), _itemsCatalog.GetAllItems());
            view.CloseButton.onClick.AddListener(ClickOnClose);

            _cts = new();
            LoadTradeDataAsync(view, _cts.Token).Forget();
        }

        void IPresenter<TradingMainView>.Hide(TradingMainView view)
        {
            view.CloseButton.onClick.RemoveListener(ClickOnClose);

            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
        }

        private async UniTask LoadTradeDataAsync(TradingMainView view, CancellationToken ct)
        {
            try
            {
                var stationId = _stationSceneDataHolder.Current.StationId;
                var tradeData = await _messenger.RequestForStationTradeData(stationId, ct);
                ct.ThrowIfCancellationRequested();

                view.SetTradeData(tradeData);
            }
            catch (System.OperationCanceledException)
            {
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Failed to load station trade data: {ex}");
            }
        }

        private void ClickOnClose()
        {
            _uiManager.CloseModal<TradingMainPopup>();
        }
    }
}
