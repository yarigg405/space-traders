using Assets.Code.UI.Infrastructure;


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
            var view = _viewsProvider.GetView<RequestDockPopupView>();
            _screenRoot.ShowView(view);

         //   _presenter.Show(view, 
        }

        void IScreen.Hide()
        {
          //  _presenter.Hide();
        }
    }
}
