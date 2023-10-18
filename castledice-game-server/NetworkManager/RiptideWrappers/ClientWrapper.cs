using Riptide;

namespace castledice_game_server.NetworkManager;

public class ClientWrapper : IMessageSender
{
    private readonly Client _client;
    
    public ClientWrapper(Client client)
    {
        _client = client;
    }
    
    public void Send(Message message)
    {
        _client.Send(message);
    }
}