using System;

namespace Assets.Code.UI.Infrastructure.Interfaces
{
    public interface IScreensProvider
    {
        IScreen GetScreen<TScreen>() where TScreen : IScreen;
        IScreen GetScreen(Type screenType);
    }
}
