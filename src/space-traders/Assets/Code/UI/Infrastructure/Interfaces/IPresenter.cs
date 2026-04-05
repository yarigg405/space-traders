namespace Assets.Code.UI.Infrastructure.Interfaces
{
    public interface IPresenter<TView> where TView : UIScreenView
    {
        void Show(TView view, object args);
        void Hide(TView view);
    }
}
