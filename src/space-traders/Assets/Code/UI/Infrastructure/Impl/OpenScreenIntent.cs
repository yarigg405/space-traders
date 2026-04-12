using Assets.Code.UI.Infrastructure.Interfaces;
using System;


namespace Assets.Code.UI.Infrastructure.Impl
{
    internal sealed class OpenScreenIntent: IScreenIntent
    {
        private readonly Type _type;
        private readonly object _args;

        public OpenScreenIntent(Type type, object args)
        {
            _type = type;
            _args = args;
        }

        public IScreen Execute(IScreensProvider provider)
        {
            var screen = provider.GetScreen(_type);
            screen.Show(_args);
            return screen;
        }

        public IScreen GetScreen(IScreensProvider provider)
        {
            return provider.GetScreen(_type);
        }
    }
}
