using Riptide;

namespace castledice_game_server.NetworkManager.RiptideWrappers;

public interface IMessageSenderById
{
    void Send(Message message, ushort clientId);
    void SendToAll(Message message);
    void SendToAll(Message message, ushort except);
}