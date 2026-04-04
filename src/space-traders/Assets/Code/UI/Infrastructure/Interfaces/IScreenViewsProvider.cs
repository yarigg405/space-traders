namespace Assets.Code.UI.Infrastructure.Interfaces
{
    public interface IScreenViewsProvider
    {
        TView GetView<TView>() where TView : UIScreenView;
    }
}