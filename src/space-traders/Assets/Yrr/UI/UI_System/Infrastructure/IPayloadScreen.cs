using System;


namespace Yrr.UI.Infrastructure
{
    internal interface IPayloadScreen<TPayload> : IUIScreen
    {
        void Show(TPayload payload, Action closingCallback);
    }
}
