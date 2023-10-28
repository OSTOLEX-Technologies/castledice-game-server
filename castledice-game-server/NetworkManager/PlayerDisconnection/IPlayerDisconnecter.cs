namespace castledice_game_server.NetworkManager.PlayerDisconnection;

public interface IPlayerDisconnecter
{
    void DisconnectPlayerWithId(int playerId);
}