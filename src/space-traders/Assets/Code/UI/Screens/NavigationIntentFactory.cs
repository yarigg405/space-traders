using Assets.Code.UI.Infrastructure.Impl;
using Assets.Code.UI.Infrastructure.Interfaces;
using Assets.Code.UI.Screens.MenuScene;
using System;
using VContainer;


namespace Assets.Code.UI.Screens
{
    public sealed class NavigationIntentFactory : INavigationIntentFactory
    {
        private readonly IObjectResolver _objectResolver;

        public NavigationIntentFactory(IObjectResolver objectResolver)
        {
            _objectResolver = objectResolver;
        }

        INavigationRequest INavigationIntentFactory.Create(Type screenType, object args)
        {
            if (screenType == typeof(SelectCharacterScreen))
                return _objectResolver.Resolve<SelectCharacterIntent>();

            return new OpenScreenIntent(screenType, args);
        }
    }
}
