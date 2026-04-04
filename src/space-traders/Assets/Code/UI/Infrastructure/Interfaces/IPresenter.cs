using Assets.Code.UI.Infrastructure;


namespace Assets.Code.UI.Infrastructure.Interfaces
{
    public interface IPresenter<TView> where TView : UIScreenView
    {
        void Show(TView view);
        void Hide(TView view);
    }
}
