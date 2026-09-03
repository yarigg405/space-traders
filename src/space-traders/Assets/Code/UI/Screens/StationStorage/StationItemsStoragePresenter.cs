using Assets.Code.ClientPart.Networking;
using Assets.Code.Common.Inventory;
using Assets.Code.Common.StaticData;
using Assets.Code.Infrastructure.Loading;
using Assets.Code.Networking.Data;
using Assets.Code.UI.DragAndDrop;
using Assets.Code.UI.Elements;
using Assets.Code.UI.Infrastructure.Interfaces;
using Assets.Code.UI.Screens.BuySellPopups;
using Assets.Code.UI.Screens.ItemInfo;
using Assets.Code.UI.Screens.StationsInventory;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Yrr.Utils;


namespace Assets.Code.UI.Screens.StationStorage
{
    public sealed class StationItemsStoragePresenter : IPresenter<StationItemsStorageView>
    {
        private readonly IUIManager _uiManager;
        private readonly ClientMessenger _messenger;
        private readonly IItemsCatalog _itemsCatalog;
        private readonly ContextMenuController _contextMenu;
        private readonly ItemDragController _dragController;
        private readonly StationSceneDataHolder _stationSceneDataHolder;

        private StationItemsStorageView _view;
        private CancellationTokenSource _cts;

        public StationItemsStoragePresenter(IUIManager uiManager, ClientMessenger messenger,
            IItemsCatalog itemsCatalog, ContextMenuController contextMenu,
            ItemDragController dragController, StationSceneDataHolder stationSceneDataHolder)
        {
            _uiManager = uiManager;
            _messenger = messenger;
            _itemsCatalog = itemsCatalog;
            _contextMenu = contextMenu;
            _dragController = dragController;
            _stationSceneDataHolder = stationSceneDataHolder;
        }

        void IPresenter<StationItemsStorageView>.Show(StationItemsStorageView view, object args)
        {
            _view = view;

            view.CloseButton.onClick.AddListener(Close);

            _contextMenu.Close();

            RequestInventory();
        }

        void IPresenter<StationItemsStorageView>.Hide(StationItemsStorageView view)
        {
            view.CloseButton.onClick.RemoveListener(Close);

            _contextMenu.Close();

            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;

            view.TilesRoot.ClearChildren();
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
                    BuildTiles(FindStationItems(data));
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to load station storage: {ex}");
            }
        }

        private IReadOnlyList<InventoryItemData> FindStationItems(PlayerInventoryData data)
        {
            if (data.Stations == null)
                return null;

            var stationId = _stationSceneDataHolder.Current.StationId;

            foreach (var station in data.Stations)
                if (station.StationId == stationId)
                    return station.Items;

            return null;
        }

        private void BuildTiles(IReadOnlyList<InventoryItemData> items)
        {
            _view.TilesRoot.ClearChildren();

            if (items == null)
                return;

            var stationId = _stationSceneDataHolder.Current.StationId;

            foreach (var item in items)
            {
                if (!TryGetItem(item.ItemId, out var itemSo))
                    continue;

                var tile = UnityEngine.Object.Instantiate(_view.TilePrefab, _view.TilesRoot);
                tile.Bind(itemSo, item.Amount, stationId, OnItemContext, _dragController);
            }
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

        private void OnItemContext(ItemSO item, int amount, Vector2 position)
        {
            var entries = new List<ContextMenuEntry>
            {
                new(ContextMenuKeys.ItemInfo, () => OpenInfo(item)),
                new(ContextMenuKeys.ItemSell, () => OpenSell(item, amount)),
            };

            _contextMenu.Open(position, entries);
        }

        private void OpenInfo(ItemSO item)
        {
            _uiManager.OpenModal<ItemInfoPopup>(new ItemInfoArgs(item));
        }

        private void OpenSell(ItemSO item, int amount)
        {
            var station = _stationSceneDataHolder.Current;

            _uiManager.OpenModal<SellItemPopup>(new SellItemArgs(
                item, station.StationId, station.StationName, station.StarSystemName, amount, RequestInventory));
        }

        private void Close()
        {
            _uiManager.CloseModal<StationItemsStoragePopup>();
        }
    }
}
