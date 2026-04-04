using Assets.Code.UI.Infrastructure;
using Assets.Code.UI.Infrastructure.Interfaces;


namespace Assets.Code.UI
{
    public abstract class ScreenBase<TPresenter, TView> : IScreen
        where TView: UIScreenView
        where TPresenter : IPresenter<TView>
    {
        private readonly IScreenViewsProvider _viewsProvider;
        private readonly TPresenter _presenter;
        private readonly LayerUI _screenRoot;

        protected ScreenBase(IScreenViewsProvider viewsProvider, 
            TPresenter presenter, LayerUI screenRoot)
        {
            _viewsProvider = viewsProvider;
            _presenter = presenter;
            _screenRoot = screenRoot;
        }


        void IScreen.Show(object args)
        {
            var view = _viewsProvider.GetView<TView>();

            _presenter.Show(view);
            _screenRoot.ShowView(view);
        }

        void IScreen.Hide()
        {
            var view = _viewsProvider.GetView<TView>();
            _presenter.Hide(view);
            _screenRoot.HideView(view);
        }
    }
}
