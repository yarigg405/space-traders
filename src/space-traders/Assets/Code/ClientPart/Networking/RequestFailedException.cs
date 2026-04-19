using System;


namespace Assets.Code.ClientPart.Networking
{
    public sealed class RequestFailedException : Exception
    {
        public RequestFailedException(string message) : base(message) { }
    }
}
