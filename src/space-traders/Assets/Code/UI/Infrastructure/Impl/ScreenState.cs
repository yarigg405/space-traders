using System;


namespace Assets.Code.UI.Infrastructure.Impl
{
    internal readonly struct ScreenState
    {
        public readonly Type ScreenType;
        public readonly object Args;

        public ScreenState(Type screenType, object args)
        {
            ScreenType = screenType;
            Args = args;
        }
    }
}
