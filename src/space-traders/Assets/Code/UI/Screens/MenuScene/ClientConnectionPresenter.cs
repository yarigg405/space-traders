using Assets.Code.UI.Infrastructure.Interfaces;
using Assets.Code.UI.Screens.MainMenu;


namespace Assets.Code.UI.Screens.MenuScene
{
    public sealed class ClientConnectionPresenter : IPresenter<ClientConnectionView>
    {
        private readonly IUIManager _uIManager;

        public ClientConnectionPresenter(IUIManager uIManager)
        {
            _uIManager = uIManager;
        }

        void IPresenter<ClientConnectionView>.Show(ClientConnectionView view)
        {
            view.CloseButton.onClick.AddListener(ClickOnBack);

            view.Show();
        }

        void IPresenter<ClientConnectionView>.Hide(ClientConnectionView view)
        {
            view.Hide();

            view.CloseButton.onClick.RemoveListener(ClickOnBack);
        }

        private void ClickOnBack()
        {
            _uIManager.BackToPreviousScreen();
        }
    }
}
