using Riptide;

namespace castledice_game_server.NetworkManager.RiptideWrappers;

public class ServerWrapper : IMessageSenderById, IClientDisconnecter
{
    private readonly Server _server;

    public ServerWrapper(Server server)
    {
        _server = server;
    }

    public void Send(Message message, ushort clientId)
    {
        _server.Send(message, clientId);
    }

    public void SendToAll(Message message)
    {
        _server.SendToAll(message);
    }

    public void SendToAll(Message message, ushort except)
    {
        _server.SendToAll(message, except);
    }

    public void DisconnectClient(ushort clientId, Message message = null)
    {
        _server.DisconnectClient(clientId, message);
    }
}