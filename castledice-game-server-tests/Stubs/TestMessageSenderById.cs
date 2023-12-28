using castledice_game_server.NetworkManager;
using castledice_game_server.NetworkManager.RiptideWrappers;
using Riptide;

namespace castledice_game_server_tests.TestImplementations;

public class TestMessageSenderById : IMessageSenderById
{
    public Message SentMessage { get; set; }
    
    public void Send(Message message, ushort clientId)
    { 
        SentMessage = message;
    }

    public void SendToAll(Message message)
    {
        SentMessage = message;
    }

    public void SendToAll(Message message, ushort except)
    {
        SentMessage = message;
    }
}