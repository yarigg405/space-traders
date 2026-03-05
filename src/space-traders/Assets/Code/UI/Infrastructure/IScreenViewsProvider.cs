namespace Assets.Code.UI.Infrastructure
{
    public interface IScreenViewsProvider
    {
        TView GetView<TView>() where TView : UIScreenView;
    }
}