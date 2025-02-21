using API.signalRModels;
namespace API.Hubs.Client
{
    public interface IStreamHub
    {
        Task ReceiveMessage(ChatMessage message);
        Task ReceivePeerGroup(PeerGroupRequest group);
        Task ReceivePeerGroupNotFound(PeerGroupRequest group);
        Task ReceiveNewPeer(PeerConnectionRequest peer);
        Task ReceiveNewInitiator(SignalRequest peer);
        Task ReceiveSignal(SignalRequest stream);
        Task ReceivePeerDisconnected(DisconnectionResponse peer);
    }
}