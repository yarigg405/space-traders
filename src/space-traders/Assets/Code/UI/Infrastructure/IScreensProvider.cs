namespace Assets.Code.UI.Infrastructure
{
    public interface IScreensProvider
    {
        IScreen GetScreen<TScreen>() where TScreen : IScreen;
    }
}
