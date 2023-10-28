namespace castledice_game_server.NetworkManager.PlayersTracking;

public interface IPlayerClientIdRemover
{
    bool RemoveClientIdForPlayer(int playerId);
}