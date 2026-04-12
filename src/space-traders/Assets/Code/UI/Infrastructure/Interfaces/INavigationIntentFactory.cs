using System;


namespace Assets.Code.UI.Infrastructure.Interfaces
{
    public interface INavigationIntentFactory
    {
        INavigationRequest Create(Type screenType, object args);
    }
}
