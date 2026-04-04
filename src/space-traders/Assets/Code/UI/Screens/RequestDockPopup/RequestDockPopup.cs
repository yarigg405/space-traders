using Assets.Code.Common.DataContainers;
using Assets.Code.UI.Infrastructure;
using Assets.Code.UI.Infrastructure.Interfaces;
using Assets.Code.UI.Layers;


namespace Assets.Code.UI.Screens
{
    public sealed class RequestDockPopup : IScreen
    {
        private readonly IScreenViewsProvider _viewsProvider;
        private readonly LayerUI_Popups _screenRoot;
        private readonly RequestDockPopupPresenter _presenter;

        public RequestDockPopup(IScreenViewsProvider viewsProvider,
            LayerUI_Popups screenRoot, RequestDockPopupPresenter presenter)
        {
            _viewsProvider = viewsProvider;
            _screenRoot = screenRoot;
            _presenter = presenter;
        }

        void IScreen.Show(object args)
        {
            var dockingData = (DockingDataContainer)args;
            var view = _viewsProvider.GetView<RequestDockPopupView>();

            _screenRoot.ShowView(view);
            _presenter.Show(view, dockingData);
        }

        void IScreen.Hide()
        {
            var view = _viewsProvider.GetView<RequestDockPopupView>();

            _presenter.Hide(view);
            _screenRoot.HideView(view);
        }
    }
}
