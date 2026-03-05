using VContainer;


namespace Assets.Code.UI.Infrastructure.Impl
{
    public sealed class ScreensProvider : IScreensProvider
    {
        private readonly IObjectResolver _objectResolver;

        public ScreensProvider(IObjectResolver objectResolver)
        {
            _objectResolver = objectResolver;
        }

        public IScreen GetScreen<TScreen>() where TScreen : IScreen
        {
            return _objectResolver.Resolve<TScreen>();
        }
    }
}
