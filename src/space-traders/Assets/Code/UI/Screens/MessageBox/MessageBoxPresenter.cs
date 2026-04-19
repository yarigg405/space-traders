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
            if (args is not MessageBoxData data)
            {
                Debug.LogError("MessageBox need MessageBoxData");
                return;
            }

            view.CloseButton.onClick.AddListener(ClosePopup);
            view.SetMessage("LocalizationTable",  data.Message, data.Args);
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
