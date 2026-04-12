namespace Assets.Code.UI.Infrastructure.Interfaces
{
    public interface INavigationIntent : INavigationRequest
    {
        IScreen Execute(IScreensProvider provider);
    }
}
