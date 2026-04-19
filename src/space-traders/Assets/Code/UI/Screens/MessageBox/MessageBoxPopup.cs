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

        public static MessageBoxData CreateData(string message, params object[] args)
        {
            return new MessageBoxData(message, args);
        }
    }

    public readonly struct MessageBoxData
    {
        public readonly string Message;
        public readonly object[] Args;

        public MessageBoxData(string message, params object[] args)
        {
            Message = message;
            Args = args;
        }
    }
}
