using Assets.Code.Common.Inventory;
using Assets.Code.UI.Infrastructure.Interfaces;
using Assets.Code.UI.Layers;


namespace Assets.Code.UI.Screens.ItemInfo
{
    public sealed class ItemInfoPopup : ScreenBase<ItemInfoPresenter, ItemInfoView>
    {
        public ItemInfoPopup(IScreenViewsProvider viewsProvider,
            ItemInfoPresenter presenter, LayerUI_Popups screenRoot)
            : base(viewsProvider, presenter, screenRoot) { }
    }

    public readonly struct ItemInfoArgs
    {
        public readonly ItemSO Item;

        public ItemInfoArgs(ItemSO item)
        {
            Item = item;
        }
    }
}
