using Assets.Code.ClientPart.Networking;
using Assets.Code.Common.Inventory;
using Assets.Code.Common.StaticData;
using Assets.Code.Networking.Data;
using Assets.Code.UI.Elements;
using Assets.Code.UI.Infrastructure.Interfaces;
using Assets.Code.UI.Screens.BuySellPopups;
using Assets.Code.UI.Screens.ItemInfo;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;


namespace Assets.Code.UI.Screens.StationsInventory
{
    public sealed class AllAssetsPresenter : IPresenter<AllAssetsView>
    {
        private readonly IUIManager _uiManager;
        private readonly ClientMessenger _messenger;
        private readonly IItemsCatalog _itemsCatalog;
        private readonly ContextMenuController _contextMenu;

        private AllAssetsView _view;
        private CancellationTokenSource _cts;

        public AllAssetsPresenter(IUIManager uiManager, ClientMessenger messenger,
            IItemsCatalog itemsCatalog, ContextMenuController contextMenu)
        {
            _uiManager = uiManager;
            _messenger = messenger;
            _itemsCatalog = itemsCatalog;
            _contextMenu = contextMenu;
        }

        void IPresenter<AllAssetsView>.Show(AllAssetsView view, object args)
        {
            _view = view;

            view.CloseButton.onClick.AddListener(Close);
            view.ItemInfoRequested += OnItemInfo;
            view.ItemContextRequested += OnItemContext;

            _contextMenu.Close();

            RequestInventory();
        }

        void IPresenter<AllAssetsView>.Hide(AllAssetsView view)
        {
            view.CloseButton.onClick.RemoveListener(Close);
            view.ItemInfoRequested -= OnItemInfo;
            view.ItemContextRequested -= OnItemContext;

            _contextMenu.Close();

            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
            _view = null;
        }

        private void RequestInventory()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = new();

            LoadAsync(_cts.Token).Forget();
        }

        private async UniTask LoadAsync(CancellationToken ct)
        {
            try
            {
                var data = await _messenger.RequestForPlayerInventory(ct);
                ct.ThrowIfCancellationRequested();

                if (_view != null)
                    _view.Setup(BuildStations(data));
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to load player inventory: {ex}");
            }
        }

        private IReadOnlyList<AllAssetsStationModel> BuildStations(PlayerInventoryData data)
        {
            var stations = new List<AllAssetsStationModel>();

            if (data.Stations == null)
                return stations;

            foreach (var station in data.Stations)
            {
                var items = new List<AllAssetsItemModel>();

                if (station.Items != null)
                {
                    foreach (var item in station.Items)
                    {
                        if (!TryGetItem(item.ItemId, out var itemSo))
                            continue;

                        items.Add(new AllAssetsItemModel(itemSo, item.Amount));
                    }
                }

                stations.Add(new AllAssetsStationModel(
                    station.StationId, station.StationName, station.SystemName, items));
            }

            return stations;
        }

        private bool TryGetItem(string itemId, out ItemSO item)
        {
            try
            {
                item = _itemsCatalog.GetItem(itemId);
                return item != null;
            }
            catch (Exception)
            {
                Debug.LogError($"Item '{itemId}' is missing from the catalog and was skipped.");
                item = null;
                return false;
            }
        }

        private void OnItemInfo(ItemSO item) => OpenInfo(item);

        private void OnItemContext(AllAssetsContextRequest request)
        {
            var entries = new List<ContextMenuEntry>
            {
                new(ContextMenuKeys.ItemInfo, () => OpenInfo(request.Item)),
                new(ContextMenuKeys.ItemSell, () => OpenSell(request)),
            };

            _contextMenu.Open(request.Position, entries);
        }

        private void OpenInfo(ItemSO item)
        {
            _uiManager.OpenModal<ItemInfoPopup>(new ItemInfoArgs(item));
        }

        private void OpenSell(AllAssetsContextRequest request)
        {
            _uiManager.OpenModal<SellItemPopup>(new SellItemArgs(
                request.Item, request.StationId, request.StationName, request.SystemName,
                request.Amount, RequestInventory));
        }

        private void Close()
        {
            _uiManager.CloseModal<AllAssetsPopup>();
        }
    }
}
