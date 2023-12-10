using Riptide;

namespace castledice_game_server.NetworkManager.RiptideWrappers;

public interface IClientDisconnecter
{
    void DisconnectClient(ushort clientId, Message message = null);
}