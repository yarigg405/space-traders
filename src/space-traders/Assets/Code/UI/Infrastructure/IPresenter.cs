using Assets.Code.UI.Infrastructure;


namespace Assets.Code.UI
{
    public interface IPresenter<TView> where TView : UIScreenView
    {
        void Show(TView view);
        void Hide(TView view);
    }
}
