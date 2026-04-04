using Assets.Code.UI.Infrastructure.Interfaces;
using System;
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

        IScreen IScreensProvider.GetScreen<TScreen>()
        {
            return _objectResolver.Resolve<TScreen>();
        }

        IScreen IScreensProvider.GetScreen(Type screenType)
        {
            return _objectResolver.Resolve(screenType, null) as IScreen;
        }
    }
}
