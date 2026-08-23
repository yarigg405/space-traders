using Assets.Code.Common.Inventory;
using Assets.Code.UI.Infrastructure.Interfaces;
using Assets.Code.UI.Layers;
using Assets.Code.UI.Screens.TradingMain;
using System;


namespace Assets.Code.UI.Screens.BuySellPopups
{
    public sealed class BuyItemPopup : ScreenBase<BuyItemPresenter, BuyItemView>
    {
        public BuyItemPopup(IScreenViewsProvider viewsProvider,
            BuyItemPresenter presenter, LayerUI_Popups screenRoot)
            : base(viewsProvider, presenter, screenRoot) { }
    }

    public readonly struct BuyItemArgs
    {
        public readonly ItemSO Item;
        public readonly TradeOrderInfo Order;
        public readonly Action OnPurchased;

        public BuyItemArgs(ItemSO item, TradeOrderInfo order, Action onPurchased)
        {
            Item = item;
            Order = order;
            OnPurchased = onPurchased;
        }
    }
}
