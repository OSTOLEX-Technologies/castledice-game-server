using castledice_game_server.NetworkManager;
using castledice_game_server.NetworkManager.RiptideWrappers;
using Riptide;

namespace castledice_game_server_tests.TestImplementations;

public class TestMessageSender : IMessageSender
{
    public Message SentMessage { get; set; }
    
    public void Send(Message message)
    {
        SentMessage = message;
    }
}