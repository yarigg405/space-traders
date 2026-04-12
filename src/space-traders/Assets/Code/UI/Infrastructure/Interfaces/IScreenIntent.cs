namespace Assets.Code.UI.Infrastructure.Interfaces
{
    public interface IScreenIntent : INavigationIntent
    {
        IScreen GetScreen(IScreensProvider provider);
    }
}
