using Riptide;

namespace castledice_game_server.NetworkManager.PlayerDisconnection;

public interface IClientDisconnecter
{
    void DisconnectClient(ushort clientId, Message message = null);
}