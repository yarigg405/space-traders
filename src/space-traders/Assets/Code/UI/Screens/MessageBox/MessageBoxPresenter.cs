using Assets.Code.UI.Infrastructure.Interfaces;


namespace Assets.Code.UI.Screens.MessageBox
{
    public sealed class MessageBoxPresenter : IPresenter<MessageBoxView>
    {
        private readonly IUIManager _uiManager;

        public MessageBoxPresenter(IUIManager uiManager)
        {
            _uiManager = uiManager;
        }

        void IPresenter<MessageBoxView>.Show(MessageBoxView view, object args)
        {
            view.CloseButton.onClick.AddListener(ClosePopup);

            if (args != null && args is string message)
            {
                view.SetMessageText(message);
            }

            else
            {
                view.SetMessageText(string.Empty);
            }

            view.Show();
        }

        void IPresenter<MessageBoxView>.Hide(MessageBoxView view)
        {
            view.CloseButton.onClick.RemoveListener(ClosePopup);
            view.Hide();
        }

        private void ClosePopup()
        {
            _uiManager.CloseModal<MessageBoxPopup>();
        }
    }
}
