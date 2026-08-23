using Assets.Code.UI.Infrastructure.Interfaces;
using UnityEngine;


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
            if (args is MessageBoxData data)
            {
                view.CloseButton.onClick.AddListener(ClosePopup);
                view.SetMessage(data.Message, "LocalizationTable", data.Args);
                view.Show();
                return;
            }

            if (args is string message)
            {
                view.CloseButton.onClick.AddListener(ClosePopup);
                view.SetMessage(message, "LocalizationTable");
                view.Show();
                return;
            }

            Debug.LogError("MessageBox need MessageBoxData or string");
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
