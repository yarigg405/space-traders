using Assets.Code.UI.Infrastructure.Interfaces;


namespace Assets.Code.UI.Infrastructure.Impl
{
    internal static class NavigationIntentExtensions
    {
        public static IScreen ExecuteHide(this INavigationIntent intent, IScreensProvider provider)
        {
            var screen = intent.PeekScreen(provider);
            screen?.Hide();
            return screen;
        }

        public static IScreen PeekScreen(this INavigationIntent intent, IScreensProvider provider)
        {
            return intent switch
            {
                IScreenIntent screenIntent => screenIntent.GetScreen(provider),
                _ => null
            };
        }
    }
}
