using System;
using System.Collections.Generic;
using Yrr.UI.Infrastructure;


namespace Yrr.UI
{
    public sealed class ScreensSupplier : IScreenSupplier<Type>
    {
        private readonly Dictionary<Type, IUIScreen> _screens;

        IUIScreen IScreenSupplier<Type>.GetScreen<TKey>()
        {
            var key = typeof(TKey);
            return _screens[key];
        }

        public ScreensSupplier(IUIScreen[] screens)
        {
            _screens = new();

            foreach (var screen in screens)
            {
                _screens.Add(screen.GetType(), screen);
            }
        }
    }
}
