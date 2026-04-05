using Assets.Code.ServerPart.Networking;
using Riptide;


namespace Assets.Code.Networking.RequestSystem
{
    public interface IResponse
    {
        ServerToClientMessageType MessageType { get; }
        void Deserialize(Message message);
    }
}
