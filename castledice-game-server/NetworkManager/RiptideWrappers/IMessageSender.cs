using Riptide;

namespace castledice_game_server.NetworkManager.RiptideWrappers;

public interface IMessageSender
{
    void Send(Message message);
}