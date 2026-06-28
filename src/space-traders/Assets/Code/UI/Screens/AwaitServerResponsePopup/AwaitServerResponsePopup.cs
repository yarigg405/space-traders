using Assets.Code.UI.Infrastructure;
using Assets.Code.UI.Infrastructure.Interfaces;
using Assets.Code.UI.Layers;


namespace Assets.Code.UI.Screens
{
    public sealed class AwaitServerResponsePopup : IScreen
    {
        private readonly LayerUI_Popups _screenRoot;
        private readonly IScreenViewsProvider _viewsProvider;

        public AwaitServerResponsePopup(LayerUI_Popups screenRoot, IScreenViewsProvider viewsProvider)
        {
            _screenRoot = screenRoot;
            _viewsProvider = viewsProvider;
        }

        void IScreen.Show(object args)
        {
            var view = _viewsProvider.GetView<AwaitServerResponseView>();
            _screenRoot.ShowView(view);
        }

        void IScreen.Hide()
        {
            if (!_viewsProvider.TryGetView<AwaitServerResponseView>(out var view))
                return;

            _screenRoot.HideView(view);
        }
    }
}
