using Riptide;

namespace castledice_game_server.NetworkManager;

public interface IMessageSender
{
    void Send(Message message);
}