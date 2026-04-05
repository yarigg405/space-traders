using Assets.Code.ClientPart.Networking;
using Riptide;


namespace Assets.Code.Networking.RequestSystem
{
    public abstract class BaseRequest<TResponse> : IRequest<TResponse>
        where TResponse : IResponse, new()
    {
        public abstract ClientToServerMessageType MessageType { get; }
        public abstract void Serialize(Message message);
        public virtual TResponse CreateResponse() => new TResponse();
    }
}
