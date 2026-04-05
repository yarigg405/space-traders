using Assets.Code.UI.Infrastructure.Interfaces;
using Assets.Code.UI.Layers;


namespace Assets.Code.UI.Screens.MessageBox
{
    public sealed class MessageBoxPopup : ScreenBase<MessageBoxPresenter, MessageBoxView>
    {
        public MessageBoxPopup(
            IScreenViewsProvider viewsProvider,
            MessageBoxPresenter presenter,
            LayerUI_Popups screenRoot)
            : base(viewsProvider, presenter, screenRoot) { }
    }
}
