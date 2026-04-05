using Assets.Code.ClientPart.Networking;
using Riptide;


namespace Assets.Code.Networking.RequestSystem
{
    public interface IRequest<TResponse>
    {
        ClientToServerMessageType MessageType { get; }
        void Serialize(Message message);
    }
}
