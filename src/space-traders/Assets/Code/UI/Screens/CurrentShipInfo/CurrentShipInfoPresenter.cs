using Assets.Code.UI.Infrastructure.Interfaces;


namespace Assets.Code.UI.Screens.CurrentShipInfo
{
    public sealed class CurrentShipInfoPresenter : IPresenter<CurrentShipInfoView>
    {
        private readonly IUIManager _uiManager;

        public CurrentShipInfoPresenter(IUIManager uiManager)
        {
            _uiManager = uiManager;
        }

        void IPresenter<CurrentShipInfoView>.Show(CurrentShipInfoView view, object args)
        {
            view.CloseButton.onClick.AddListener(Close);
        }

        void IPresenter<CurrentShipInfoView>.Hide(CurrentShipInfoView view)
        {
            view.CloseButton.onClick.RemoveListener(Close);
        }

        private void Close()
        {
            _uiManager.CloseModal<CurrentShipInfoScreen>();
        }
    }
}
