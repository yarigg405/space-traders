using System;


namespace Yrr.UI.Infrastructure
{
    internal interface IUiSimpleScreen : IUIScreen
    {
        void Show(Action closingCallback);
    }
}
