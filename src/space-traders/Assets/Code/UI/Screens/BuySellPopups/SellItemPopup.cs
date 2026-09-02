using Assets.Code.Common.Inventory;
using Assets.Code.UI.Infrastructure.Interfaces;
using Assets.Code.UI.Layers;
using System;


namespace Assets.Code.UI.Screens.BuySellPopups
{
    public sealed class SellItemPopup : ScreenBase<SellItemPresenter, SellItemView>
    {
        public SellItemPopup(IScreenViewsProvider viewsProvider,
            SellItemPresenter presenter, LayerUI_Popups screenRoot)
            : base(viewsProvider, presenter, screenRoot) { }
    }

    public readonly struct SellItemArgs
    {
        public readonly ItemSO Item;
        public readonly int StationId;
        public readonly string StationName;
        public readonly string SystemName;
        public readonly int Available;
        public readonly Action OnSold;

        public SellItemArgs(ItemSO item, int stationId, string stationName, string systemName,
            int available, Action onSold)
        {
            Item = item;
            StationId = stationId;
            StationName = stationName;
            SystemName = systemName;
            Available = available;
            OnSold = onSold;
        }
    }
}
